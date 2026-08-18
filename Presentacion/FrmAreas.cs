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
    public partial class FrmAreas : Form
    {
        private bool ordenAscendente = true;
        private string columnaOrdenada = null;
        private readonly AuditoriaLN auditoriaLN = new AuditoriaLN();
        private readonly AreaLN areaLN = new AreaLN();
        private readonly Usuario usuarioActual;
        private List<Area> listaAreas;
        private Area areaSeleccionada;
        private List<Area> listaAreasCompleta;



        public FrmAreas(Usuario usuarioActual)
        {
            InitializeComponent();
            this.usuarioActual = usuarioActual;
            grid.SelectionChanged += grid_SelectionChanged;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            grid.ColumnHeaderMouseClick += grid_ColumnHeaderMouseClick;
            CargarGrid();
            LimpiarFormulario();
            toolTip1.SetToolTip(button1, "Limpiar el formulario para crear una nueva área");
            toolTip1.SetToolTip(button2, "Guardar los cambios del área");
            toolTip1.SetToolTip(button3, "Eliminar el área permanentemente");
        }
        private bool ValidarCampos()
        {
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtNombreArea.Text))
            {
                errorProvider1.SetError(txtNombreArea, "Este campo es obligatorio.");
                return false;
            }

            return true;
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

            var propInfo = typeof(Area).GetProperty(propiedad);
            if (propInfo == null) return;

            listaAreas = ordenAscendente
                ? listaAreas.OrderBy(a => propInfo.GetValue(a)).ToList()
                : listaAreas.OrderByDescending(a => propInfo.GetValue(a)).ToList();

            grid.DataSource = null;
            grid.DataSource = listaAreas;

            if (grid.Columns["IdArea"] != null) grid.Columns["IdArea"].Visible = false;
            if (grid.Columns["NombreArea"] != null) grid.Columns["NombreArea"].HeaderText = "Nombre del Área";
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null) return;

            areaSeleccionada = grid.CurrentRow.DataBoundItem as Area;
            if (areaSeleccionada == null) return;

            txtNombreArea.Text = areaSeleccionada.NombreArea;
        }
        private void CargarGrid()
        {
            try
            {
                listaAreasCompleta = areaLN.ShowArea();
                AplicarBusqueda();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarBusqueda()
        {
            try
            {
                string busqueda = txtBuscar.Text.Trim();

                listaAreas = string.IsNullOrWhiteSpace(busqueda)
                    ? listaAreasCompleta
                    : listaAreasCompleta
                        .Where(a => a.NombreArea.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                grid.DataSource = null;
                grid.DataSource = listaAreas;

                if (grid.Columns["IdArea"] != null) grid.Columns["IdArea"].Visible = false;
                if (grid.Columns["NombreArea"] != null) grid.Columns["NombreArea"].HeaderText = "Nombre del Área";
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                lblContador.Text = $"Total: {listaAreas.Count} área(s)";
                lblSinDatos.Text = listaAreas.Count == 0 ? "No hay áreas registradas." : "";
                lblSinDatos.Visible = listaAreas.Count == 0;
                lblSinDatos.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void FrmAreas_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();

        }
        private void LimpiarFormulario()
        {
            grid.CurrentCell = null;
            grid.ClearSelection();

            areaSeleccionada = null;
            txtNombreArea.Clear();
            txtNombreArea.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                bool esNueva = areaSeleccionada == null;

                if (esNueva)
                {
                    Area nueva = new Area(0, txtNombreArea.Text);
                    areaLN.InsertArea(nueva);

                    auditoriaLN.Registrar(
                        usuarioActual.IdUsuario,
                        $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                        "Crear",
                        "Area",
                        null,
                        $"Área creada: {txtNombreArea.Text}"
                    );
                }
                else
                {
                    string nombreAnterior = areaSeleccionada.NombreArea;
                    areaSeleccionada.NombreArea = txtNombreArea.Text;
                    areaLN.UpdateArea(areaSeleccionada);

                    auditoriaLN.Registrar(
                        usuarioActual.IdUsuario,
                        $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                        "Modificar",
                        "Area",
                        areaSeleccionada.IdArea,
                        $"Área: {nombreAnterior} → {areaSeleccionada.NombreArea}"
                    );
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
            if (areaSeleccionada == null)
            {
                MessageBox.Show("Selecciona un área primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                $"¿Eliminar el área \"{areaSeleccionada.NombreArea}\"?\n\n" +
                "Si tiene incidencias asociadas, no se podrá eliminar.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes) return;
            try
            {
                string nombreEliminada = areaSeleccionada.NombreArea;
                int idEliminada = areaSeleccionada.IdArea;

                areaLN.DeleteArea(areaSeleccionada);

                auditoriaLN.Registrar(
                    usuarioActual.IdUsuario,
                    $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                    "Eliminar",
                    "Area",
                    idEliminada,
                    $"Área eliminada: {nombreEliminada}"
                );

                CargarGrid();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarBusqueda();

        }

        private void FrmAreas_KeyDown(object sender, KeyEventArgs e)
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

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] pdf = IncidenciaReportes.GenerarPdfListadoAreas(listaAreas);
                GuardarArchivo(pdf, "pdf", "Archivo PDF|*.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] excel = IncidenciaReportes.GenerarExcelListadoAreas(listaAreas);
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
                dialogo.FileName = $"Areas_{DateTime.Now:yyyyMMdd_HHmm}.{extension}";

                if (dialogo.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(dialogo.FileName, contenido);
                    MessageBox.Show("Exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void lblSinDatos_Click(object sender, EventArgs e)
        {

        }
    }
    }
