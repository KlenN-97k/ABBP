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
    public partial class FrmIncidencias : Form
    {
        private readonly IncidenciaLN incidenciaLN = new IncidenciaLN();
        private readonly AreaLN areaLN = new AreaLN();
        private readonly PrioridadLN prioridadLN = new PrioridadLN();
        private readonly EstadoLN estadoLN = new EstadoLN();
        private readonly UsuarioLN usuarioLN = new UsuarioLN();

        private List<Incidencia> listaIncidencias;
        private Incidencia incidenciaSeleccionada;
        public FrmIncidencias()
        {
            InitializeComponent();
            grid.SelectionChanged += grid_SelectionChanged; // aseguramos que quede enganchado
            CargarCombos();
            CargarGrid();
            LimpiarFormulario();
        }
        private void CargarCombos()
        {
            cboArea.DataSource = areaLN.ShowArea();
            cboArea.DisplayMember = "NombreArea";
            cboArea.ValueMember = "IdArea";

            cboPrioridad.DataSource = prioridadLN.ShowPrioridad();
            cboPrioridad.DisplayMember = "Nombre";
            cboPrioridad.ValueMember = "IdPrioridad";

            cboEstado.DataSource = estadoLN.ShowEstado();
            cboEstado.DisplayMember = "Nombre";
            cboEstado.ValueMember = "IdEstado";

            var tecnicos = usuarioLN.ShowUsuario()
                .Where(u => u.Rol == "Técnico" && u.Estado)
                .ToList();
            tecnicos.Insert(0, new Usuario { IdUsuario = 0, Nombre = "(Sin asignar)", Apellido = "" });

            cboTecnico.DataSource = tecnicos;
            cboTecnico.DisplayMember = "Nombre";
            cboTecnico.ValueMember = "IdUsuario";
        }

        private void CargarGrid()
        {
            try
            {
                listaIncidencias = incidenciaLN.ShowIncidencia();
                grid.DataSource = null;
                grid.DataSource = listaIncidencias;

                foreach (string columna in new[] { "IdIncidencia", "IdArea", "IdPrioridad", "IdEstado", "IdTecnicoAsignado", "Descripcion", "Observaciones" })
                {
                    if (grid.Columns[columna] != null) grid.Columns[columna].Visible = false;
                }

                if (grid.Columns["NumeroTicket"] != null) grid.Columns["NumeroTicket"].HeaderText = "Ticket";
                if (grid.Columns["NombreArea"] != null) grid.Columns["NombreArea"].HeaderText = "Área";
                if (grid.Columns["TipoIncidencia"] != null) grid.Columns["TipoIncidencia"].HeaderText = "Tipo";
                if (grid.Columns["NombrePrioridad"] != null) grid.Columns["NombrePrioridad"].HeaderText = "Prioridad";
                if (grid.Columns["NombreEstado"] != null) grid.Columns["NombreEstado"].HeaderText = "Estado";
                if (grid.Columns["TecnicoAsignado"] != null) grid.Columns["TecnicoAsignado"].HeaderText = "Técnico";
                if (grid.Columns["FechaSolucion"] != null) grid.Columns["FechaSolucion"].HeaderText = "Fecha Solución";

                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null) return;
            incidenciaSeleccionada = grid.CurrentRow.DataBoundItem as Incidencia;
            if (incidenciaSeleccionada == null) return;

            txtEmpleado.Text = incidenciaSeleccionada.Empleado;
            txtTipo.Text = incidenciaSeleccionada.TipoIncidencia;
            txtDescripcion.Text = incidenciaSeleccionada.Descripcion;
            txtObservaciones.Text = incidenciaSeleccionada.Observaciones;

            cboArea.SelectedValue = incidenciaSeleccionada.IdArea;
            cboPrioridad.SelectedValue = incidenciaSeleccionada.IdPrioridad;
            cboEstado.SelectedValue = incidenciaSeleccionada.IdEstado;
            cboTecnico.SelectedValue = incidenciaSeleccionada.IdTecnicoAsignado ?? 0;
        }
        private void panelFormulario_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();

        }

        private void LimpiarFormulario()
        {
            incidenciaSeleccionada = null;
            txtEmpleado.Clear();
            txtTipo.Clear();
            txtDescripcion.Clear();
            txtObservaciones.Clear();
            if (cboArea.Items.Count > 0) cboArea.SelectedIndex = 0;
            if (cboPrioridad.Items.Count > 0) cboPrioridad.SelectedIndex = 0;
            if (cboEstado.Items.Count > 0) cboEstado.SelectedIndex = 0;
            if (cboTecnico.Items.Count > 0) cboTecnico.SelectedIndex = 0;
            grid.ClearSelection();
            txtEmpleado.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int? idTecnico = (int)cboTecnico.SelectedValue == 0 ? (int?)null : (int)cboTecnico.SelectedValue;

                if (incidenciaSeleccionada == null)
                {
                    Incidencia nueva = new Incidencia(
                        0, null, DateTime.Now, txtEmpleado.Text, (int)cboArea.SelectedValue,
                        txtTipo.Text, txtDescripcion.Text, (int)cboPrioridad.SelectedValue,
                        (int)cboEstado.SelectedValue, idTecnico, null, txtObservaciones.Text
                    );
                    incidenciaLN.InsertIncidencia(nueva);
                }
                else
                {
                    incidenciaSeleccionada.Empleado = txtEmpleado.Text;
                    incidenciaSeleccionada.IdArea = (int)cboArea.SelectedValue;
                    incidenciaSeleccionada.TipoIncidencia = txtTipo.Text;
                    incidenciaSeleccionada.Descripcion = txtDescripcion.Text;
                    incidenciaSeleccionada.IdPrioridad = (int)cboPrioridad.SelectedValue;
                    incidenciaSeleccionada.IdEstado = (int)cboEstado.SelectedValue;
                    incidenciaSeleccionada.IdTecnicoAsignado = idTecnico;
                    incidenciaSeleccionada.Observaciones = txtObservaciones.Text;

                    incidenciaLN.UpdateIncidencia(incidenciaSeleccionada);
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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (incidenciaSeleccionada == null)
            {
                MessageBox.Show("Selecciona una incidencia primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                $"¿Eliminar la incidencia {incidenciaSeleccionada.NumeroTicket}?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes) return;

            try
            {
                incidenciaLN.DeleteIncidencia(incidenciaSeleccionada);
                CargarGrid();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] pdf = IncidenciaReportes.GenerarPdfListado(listaIncidencias);
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
                byte[] excel = IncidenciaReportes.GenerarExcelListado(listaIncidencias);
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
                dialogo.FileName = $"Incidencias_{DateTime.Now:yyyyMMdd_HHmm}.{extension}";

                if (dialogo.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(dialogo.FileName, contenido);
                    MessageBox.Show("Exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    
    }
}
