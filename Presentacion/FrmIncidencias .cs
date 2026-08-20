using Entidades.Gestion_de_Entidades;
using Logica.Gestion_de_Logica;
using Reportes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class FrmIncidencias : Form
    {
        private readonly AuditoriaLN auditoriaLN = new AuditoriaLN();
        private readonly IncidenciaLN incidenciaLN = new IncidenciaLN();
        private readonly AreaLN areaLN = new AreaLN();
        private readonly PrioridadLN prioridadLN = new PrioridadLN();
        private readonly EstadoLN estadoLN = new EstadoLN();
        private readonly UsuarioLN usuarioLN = new UsuarioLN();
        private readonly Usuario usuarioActual;
        private List<Incidencia> listaIncidencias;
        private List<Incidencia> listaIncidenciasCompleta;
        private Incidencia incidenciaSeleccionada;
        private FiltroIncidencias filtroActual = new FiltroIncidencias();
        private string ticketBusquedaActual = string.Empty;
        private bool ordenAscendente = true;
        private string columnaOrdenada = null;
        public FrmIncidencias(Usuario usuarioActual)
        {
            InitializeComponent();
            toolTip1.SetToolTip(btnNuevo, "Limpiar el formulario para crear una nueva incidencia");
            toolTip1.SetToolTip(btnGuardar, "Guardar los cambios de la incidencia");
            toolTip1.SetToolTip(btnEliminar, "Eliminar la incidencia seleccionada permanentemente");
            toolTip1.SetToolTip(btnExportarPdf, "Exportar el listado actual a PDF");
            toolTip1.SetToolTip(btnExportarExcel, "Exportar el listado actual a Excel");
            toolTip1.SetToolTip(btnFiltros, "Filtrar por estado o rango de fechas");
            toolTip1.SetToolTip(txtBusquedaRapida, "Buscar por número de ticket");
            this.usuarioActual = usuarioActual;
            grid.SelectionChanged += grid_SelectionChanged;
            txtBusquedaRapida.TextChanged += txtBusquedaRapida_TextChanged;
            grid.ColumnHeaderMouseClick += grid_ColumnHeaderMouseClick;  
            CargarCombos();
            AplicarRestriccionesPorRol();
            CargarGrid();
            LimpiarFormulario();
        }
        private void txtBusquedaRapida_TextChanged(object sender, EventArgs e)
        {
            ticketBusquedaActual = txtBusquedaRapida.Text.Trim();
            AplicarFiltro();
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

            var propInfo = typeof(Incidencia).GetProperty(propiedad);
            if (propInfo == null) return;

            listaIncidencias = ordenAscendente
                ? listaIncidencias.OrderBy(i => propInfo.GetValue(i)).ToList()
                : listaIncidencias.OrderByDescending(i => propInfo.GetValue(i)).ToList();

            grid.DataSource = null;
            grid.DataSource = listaIncidencias;
            AplicarFormatoColumnas();
        }
        private bool ValidarCampos()
        {
            errorProvider1.Clear();
            bool esValido = true;

            if (string.IsNullOrWhiteSpace(txtEmpleado.Text))
            {
                errorProvider1.SetError(txtEmpleado, "Este campo es obligatorio.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtTipo.Text))
            {
                errorProvider1.SetError(txtTipo, "Este campo es obligatorio.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                errorProvider1.SetError(txtDescripcion, "Este campo es obligatorio.");
                esValido = false;
            }

            return esValido;
        }
        private void AplicarRestriccionesPorRol()
        {
            if (usuarioActual.Rol == "Usuario")
            {
                cboEstado.Enabled = false;
                cboTecnico.Enabled = false;
                DeshabilitarBoton(btnEliminar);
            }
            else if (usuarioActual.Rol == "Técnico")
            {
                cboTecnico.Enabled = false;
                DeshabilitarBoton(btnEliminar);
            }
        }

        private void DeshabilitarBoton(Button boton)
        {
            boton.Enabled = false;
            boton.BackColor = Color.FromArgb(180, 180, 180);
            boton.ForeColor = Color.FromArgb(230, 230, 230);
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
                listaIncidenciasCompleta = incidenciaLN.ShowIncidencia();

                if (usuarioActual.Rol == "Usuario")
                {
                    string nombreCompleto = $"{usuarioActual.Nombre} {usuarioActual.Apellido}";
                    listaIncidenciasCompleta = listaIncidenciasCompleta
                        .Where(i => i.Empleado == nombreCompleto)
                        .ToList();
                }

                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFiltro()
        {
            try
            {
                listaIncidencias = IncidenciaReportes.Filtrar(listaIncidenciasCompleta, filtroActual);

                if (!string.IsNullOrWhiteSpace(ticketBusquedaActual))
                {
                    listaIncidencias = listaIncidencias
                        .Where(i => i.NumeroTicket != null &&
                                    i.NumeroTicket.IndexOf(ticketBusquedaActual, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                grid.DataSource = null;
                grid.DataSource = listaIncidencias;
                AplicarFormatoColumnas();
                lblSinDatos.Text = listaIncidencias.Count == 0 ? "No hay incidencias registradas." : "";
                lblSinDatos.Visible = listaIncidencias.Count == 0;
                lblSinDatos.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFormatoColumnas()
        {
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
            grid.CurrentCell = null;
            grid.ClearSelection();

            incidenciaSeleccionada = null;
            txtEmpleado.Clear();
            txtTipo.Clear();
            txtDescripcion.Clear();
            txtObservaciones.Clear();
            if (cboArea.Items.Count > 0) cboArea.SelectedIndex = 0;
            if (cboPrioridad.Items.Count > 0) cboPrioridad.SelectedIndex = 0;
            if (cboEstado.Items.Count > 0) cboEstado.SelectedIndex = 0;
            if (cboTecnico.Items.Count > 0) cboTecnico.SelectedIndex = 0;
            txtEmpleado.Focus();
        }

        private void EnviarNotificacionSiCorresponde(Incidencia incidencia, string nombreEstadoNuevo)
        {
            if (nombreEstadoNuevo != "Resuelto" && nombreEstadoNuevo != "Cerrado") return;
            if (!incidencia.IdTecnicoAsignado.HasValue) return;

            try
            {
                Usuario tecnico = usuarioLN.ShowUsuario()
                    .FirstOrDefault(u => u.IdUsuario == incidencia.IdTecnicoAsignado.Value);

                if (tecnico?.TelegramChatId == null) return; // técnico no vinculado, no se puede avisar

                string mensaje =
                    $"✅ Incidencia {incidencia.NumeroTicket} marcada como {nombreEstadoNuevo}.\n\n" +
                    $"Empleado: {incidencia.Empleado}\nTipo: {incidencia.TipoIncidencia}";

                Bot.TelegramNotificador.EnviarMensaje(tecnico.TelegramChatId.Value, mensaje);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error detectado al enviar Telegram:\n\n{ex.ToString()}", "Error Oculto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Best effort: si falla el envío, no debe romper el guardado de la incidencia.
            }
        }

        private void NotificarNuevaIncidenciaATecnicos(Incidencia incidenciaNueva)
        {
            try
            {
                // Buscamos a todos los técnicos activos que ya hayan vinculado su Telegram
                var tecnicos = usuarioLN.ShowUsuario()
                    .Where(u => u.Rol == "Técnico" && u.Estado && u.TelegramChatId.HasValue)
                    .ToList();

                if (tecnicos.Count == 0) return; // Nadie a quien notificar

                string mensaje = $"🚨 *NUEVA INCIDENCIA REPORTADA* 🚨\n\n" +
                                 $"*Ticket:* {incidenciaNueva.NumeroTicket}\n" +
                                 $"*Empleado:* {incidenciaNueva.Empleado}\n" +
                                 $"*Área:* {cboArea.Text}\n" +
                                 $"*Tipo:* {incidenciaNueva.TipoIncidencia}\n" +
                                 $"*Prioridad:* {cboPrioridad.Text}\n\n" +
                                 $"*Descripción:*\n{incidenciaNueva.Descripcion}\n\n" +
                                 $"_Por favor, ingresa al sistema de escritorio para asignarte este ticket._";

                // Le enviamos la alerta a cada técnico
                foreach (var tecnico in tecnicos)
                {
                    Bot.TelegramNotificador.EnviarMensaje(tecnico.TelegramChatId.Value, mensaje);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error detectado al enviar Telegram:\n\n{ex.ToString()}", "Error Oculto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Best effort: Si algo falla con Telegram, no debe interrumpir el guardado en WinForms

            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                int? idTecnico = (int)cboTecnico.SelectedValue == 0 ? (int?)null : (int)cboTecnico.SelectedValue;
                bool esNueva = incidenciaSeleccionada == null;

                if (esNueva)
                {
                    Incidencia nueva = new Incidencia(
                        0, null, DateTime.Now, txtEmpleado.Text, (int)cboArea.SelectedValue,
                        txtTipo.Text, txtDescripcion.Text, (int)cboPrioridad.SelectedValue,
                        (int)cboEstado.SelectedValue, idTecnico, null, txtObservaciones.Text
                    );
                    incidenciaLN.InsertIncidencia(nueva);

                    auditoriaLN.Registrar(
                        usuarioActual.IdUsuario,
                        $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                        "Crear",
                        "Incidencia",
                        null,
                        $"Empleado: {txtEmpleado.Text} | Tipo: {txtTipo.Text}"
                    );

                    // NUEVO: Buscar la incidencia recién creada para tener su número de ticket real
                    Incidencia incidenciaCreada = incidenciaLN.ShowIncidencia()
                        .Where(i => i.Empleado == txtEmpleado.Text && i.Descripcion == txtDescripcion.Text)
                        .OrderByDescending(i => i.IdIncidencia)
                        .FirstOrDefault();

                    if (incidenciaCreada != null)
                    {
                        NotificarNuevaIncidenciaATecnicos(incidenciaCreada);
                    }
                }
                else
                {
                    string estadoAnterior = incidenciaSeleccionada.NombreEstado;

                    incidenciaSeleccionada.Empleado = txtEmpleado.Text;
                    incidenciaSeleccionada.IdArea = (int)cboArea.SelectedValue;
                    incidenciaSeleccionada.TipoIncidencia = txtTipo.Text;
                    incidenciaSeleccionada.Descripcion = txtDescripcion.Text;
                    incidenciaSeleccionada.IdPrioridad = (int)cboPrioridad.SelectedValue;
                    incidenciaSeleccionada.IdEstado = (int)cboEstado.SelectedValue;
                    incidenciaSeleccionada.IdTecnicoAsignado = idTecnico;
                    incidenciaSeleccionada.Observaciones = txtObservaciones.Text;

                    incidenciaLN.UpdateIncidencia(incidenciaSeleccionada);

                    string nuevoEstado = cboEstado.Text;
                    auditoriaLN.Registrar(
                        usuarioActual.IdUsuario,
                        $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                        "Modificar",
                        "Incidencia",
                        incidenciaSeleccionada.IdIncidencia,
                        $"Ticket: {incidenciaSeleccionada.NumeroTicket} | Estado: {estadoAnterior} → {nuevoEstado}"
                    );
                    EnviarNotificacionSiCorresponde(incidenciaSeleccionada, nuevoEstado);
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
                string ticketEliminado = incidenciaSeleccionada.NumeroTicket;
                int idEliminado = incidenciaSeleccionada.IdIncidencia;

                incidenciaLN.DeleteIncidencia(incidenciaSeleccionada);

                auditoriaLN.Registrar(
                    usuarioActual.IdUsuario,
                    $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                    "Eliminar",
                    "Incidencia",
                    idEliminado,
                    $"Ticket eliminado: {ticketEliminado}"
                );

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

        private void FrmIncidencias_Load(object sender, EventArgs e)
        {

        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        

        private void btnFiltros_Click(object sender, EventArgs e)
        {
            using (FrmFiltroIncidencias dlg = new FrmFiltroIncidencias(estadoLN.ShowEstado(), filtroActual))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filtroActual = dlg.Filtro;
                    AplicarFiltro();
                }
         
            }}

        private void FrmIncidencias_KeyDown(object sender, KeyEventArgs e)
        {

            bool enCampoMultilinea = this.ActiveControl is TextBox tb && tb.Multiline;

            if (e.KeyCode == Keys.Enter && !enCampoMultilinea)
            {
                btnGuardar.PerformClick();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && !(this.ActiveControl is TextBox))
            {
                btnEliminar.PerformClick();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                CargarGrid();
                e.Handled = true;
            }
        }
    }
}
