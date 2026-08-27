using Entidades.Gestion_de_Entidades;
using Logica.Gestion_de_Logica;
using Reportes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Presentacion
{
    public partial class FrmGuias : Form
    {
        private bool ordenAscendente = true;
        private string columnaOrdenada = null;
        private List<Guia> listaGuiasCompleta;
        private readonly GuiaLN guiaLN = new GuiaLN();
        private List<Guia> listaGuias;
        private Guia guiaSeleccionada;
        private readonly Usuario usuarioActual;

        public FrmGuias(Usuario usuarioActual)
        {
            InitializeComponent();
            this.usuarioActual = usuarioActual;

            grid.SelectionChanged += grid_SelectionChanged;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            grid.ColumnHeaderMouseClick += grid_ColumnHeaderMouseClick;
            ConfigurarPlaceholder(txtCorreoDestino, "correo@ejemplo.com");
            CargarGrid();
            LimpiarFormulario();
            toolTip1.SetToolTip(button1, "Limpiar el formulario para crear una nueva guía");
            toolTip1.SetToolTip(button2, "Guardar los cambios de la guía");
            toolTip1.SetToolTip(button3, "Eliminar la guía permanentemente");
            toolTip1.SetToolTip(button4, "Exportar el catálogo de guías a PDF");
            toolTip1.SetToolTip(button5, "Exportar el catálogo de guías a Excel");

            if (usuarioActual != null && usuarioActual.Rol == "Usuario")
            {
                button1.Enabled = false; // Nuevo
                button2.Enabled = false; // Guardar
                button3.Enabled = false; // Eliminar
            }
        
        }

        private void CargarGrid()
        {
            try
            {
                listaGuiasCompleta = guiaLN.ShowGuia();
                AplicarBusqueda();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string propiedad = grid.Columns[e.ColumnIndex].DataPropertyName;
            if (string.IsNullOrEmpty(propiedad)) return;

            if (columnaOrdenada == propiedad)
                ordenAscendente = !ordenAscendente;
            else
            {
                columnaOrdenada = propiedad;
                ordenAscendente = true;
            }

            var propInfo = typeof(Guia).GetProperty(propiedad);
            if (propInfo == null) return;

            listaGuias = ordenAscendente
                ? listaGuias.OrderBy(g => propInfo.GetValue(g)).ToList()
                : listaGuias.OrderByDescending(g => propInfo.GetValue(g)).ToList();

            grid.DataSource = null;
            grid.DataSource = listaGuias;

            foreach (string columna in new[] { "IdGuia", "Problema", "Solucion" })
            {
                if (grid.Columns[columna] != null) grid.Columns[columna].Visible = false;
            }
            if (grid.Columns["Titulo"] != null) grid.Columns["Titulo"].HeaderText = "Título";
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void AplicarBusqueda()
        {
            try
            {
                string busqueda = txtBuscar.Text.Trim();

                listaGuias = string.IsNullOrWhiteSpace(busqueda)
                    ? listaGuiasCompleta
                    : listaGuiasCompleta
                        .Where(g => g.Titulo.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                grid.DataSource = null;
                grid.DataSource = listaGuias;

                foreach (string columna in new[] { "IdGuia", "Problema", "Solucion" })
                {
                    if (grid.Columns[columna] != null) grid.Columns[columna].Visible = false;
                }
                if (grid.Columns["Titulo"] != null) grid.Columns["Titulo"].HeaderText = "Título";
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                lblContador.Text = $"Total: {listaGuias.Count} guía(s)";
                lblSinDatos.Text = listaGuias.Count == 0 ? "No hay guías registradas." : "";
                lblSinDatos.Visible = listaGuias.Count == 0;
                lblSinDatos.BringToFront();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null) return;

            guiaSeleccionada = grid.CurrentRow.DataBoundItem as Guia;
            if (guiaSeleccionada == null) return;

            txtTitulo.Text = guiaSeleccionada.Titulo;
            txtProblema.Text = guiaSeleccionada.Problema;
            txtSolucion.Text = guiaSeleccionada.Solucion;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            grid.CurrentCell = null;
            grid.ClearSelection();

            guiaSeleccionada = null;
            txtTitulo.Clear();
            txtProblema.Clear();
            txtSolucion.Clear();
            txtTitulo.Focus();
        }
        private bool ValidarCampos()
        {
            errorProvider1.Clear();
            bool esValido = true;

            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                errorProvider1.SetError(txtTitulo, "Este campo es obligatorio.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtProblema.Text))
            {
                errorProvider1.SetError(txtProblema, "Este campo es obligatorio.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtSolucion.Text))
            {
                errorProvider1.SetError(txtSolucion, "Este campo es obligatorio.");
                esValido = false;
            }

            return esValido;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                if (guiaSeleccionada == null)
                {
                    Guia nueva = new Guia(0, txtTitulo.Text, txtProblema.Text, txtSolucion.Text);
                    guiaLN.InsertGuia(nueva);
                }
                else
                {
                    guiaSeleccionada.Titulo = txtTitulo.Text;
                    guiaSeleccionada.Problema = txtProblema.Text;
                    guiaSeleccionada.Solucion = txtSolucion.Text;
                    guiaLN.UpdateGuia(guiaSeleccionada);
                }

                CargarGrid();
                LimpiarFormulario();
                MessageBox.Show("Guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (guiaSeleccionada == null)
            {
                MessageBox.Show("Selecciona una guía primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                $"¿Eliminar la guía \"{guiaSeleccionada.Titulo}\"?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes) return;

            try
            {
                guiaLN.DeleteGuia(guiaSeleccionada);
                CargarGrid();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] pdf = IncidenciaReportes.GenerarPdfListadoGuias(listaGuias);
                GuardarArchivo(pdf, "pdf", "Archivo PDF|*.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] excel = IncidenciaReportes.GenerarExcelListadoGuias(listaGuias);
                GuardarArchivo(excel, "xlsx", "Archivo Excel|*.xlsx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    

        private void GuardarArchivo(byte[] contenido, string extension, string filtro)
        {
            using (SaveFileDialog dialogo = new SaveFileDialog())
            {
                dialogo.Filter = filtro;
                dialogo.FileName = $"Guias_{DateTime.Now:yyyyMMdd_HHmm}.{extension}";

                if (dialogo.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(dialogo.FileName, contenido);
                    MessageBox.Show("Exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarBusqueda();

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void FrmGuias_KeyDown(object sender, KeyEventArgs e)
        {
            bool enCampoMultilinea = this.ActiveControl is TextBox tb && tb.Multiline;

            if (e.KeyCode == Keys.Enter && !enCampoMultilinea)
            {
                button2.PerformClick();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && !(this.ActiveControl is TextBox))
            {
                button3.PerformClick();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                AplicarBusqueda();
                e.Handled = true;
            }
        }
        private void ConfigurarPlaceholder(TextBox txt, string textoPlaceholder)
        {
            txt.Text = textoPlaceholder;
            txt.ForeColor = Color.Gray;

            txt.Enter += (s, e) =>
            {
                if (txt.Text == textoPlaceholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = textoPlaceholder;
                    txt.ForeColor = Color.Gray;
                }
            };
        }
        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCorreoDestino.Text) || txtCorreoDestino.Text == "correo@ejemplo.com")
            {
                MessageBox.Show("Ingresa un correo de destino.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                byte[] pdf = IncidenciaReportes.GenerarPdfListadoGuias(listaGuias);

                CorreoService.EnviarCorreoConAdjunto(
                    txtCorreoDestino.Text.Trim(),
                    "Guías de Ayuda - Sistema de Incidencias APPB",
                    "Adjunto encontrarás el catálogo de guías rápidas de solución a problemas frecuentes.",
                    pdf,
                    "GuiasDeAyuda.pdf"
                );

                MessageBox.Show("Correo enviado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo enviar el correo:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
