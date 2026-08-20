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

    public partial class FrmUsuarios : Form
    {
        private bool ordenAscendente = true;
        private string columnaOrdenada = null;
        private List<Usuario> listaUsuariosCompleta;
        private readonly Usuario usuarioActual;
        private readonly AuditoriaLN auditoriaLN = new AuditoriaLN();
        private readonly UsuarioLN usuarioLN = new UsuarioLN();
        private List<Usuario> listaUsuarios;
        private Usuario usuarioSeleccionado;
        public FrmUsuarios(Usuario usuarioActual)
        {
            InitializeComponent();
            this.usuarioActual = usuarioActual;
            grid.SelectionChanged += grid_SelectionChanged;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            grid.ColumnHeaderMouseClick += grid_ColumnHeaderMouseClick;
            cboRol.Items.Clear();
            cboRol.Items.AddRange(new object[] { "Administrador", "Técnico", "Usuario" });
            toolTip1.SetToolTip(btnNuevo, "Limpiar el formulario para crear un nuevo usuario");
            toolTip1.SetToolTip(btnGuardar, "Guardar los cambios del usuario");
            toolTip1.SetToolTip(btnEliminar, "Eliminar el usuario permanentemente");
            toolTip1.SetToolTip(btnResetPassword, "Generar una contraseña temporal aleatoria");
            toolTip1.SetToolTip(txtPassword, "Dejar vacío para no cambiar la contraseña");
            CargarGrid();
            LimpiarFormulario();
        }

        private void CargarGrid()
        {
            try
            {
                listaUsuariosCompleta = usuarioLN.ShowUsuario();
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

            var propInfo = typeof(Usuario).GetProperty(propiedad);
            if (propInfo == null) return;

            listaUsuarios = ordenAscendente
                ? listaUsuarios.OrderBy(u => propInfo.GetValue(u)).ToList()
                : listaUsuarios.OrderByDescending(u => propInfo.GetValue(u)).ToList();

            grid.DataSource = null;
            grid.DataSource = listaUsuarios;

            foreach (string columna in new[] { "IdUsuario", "Password", "TelegramChatId" })
            {
                if (grid.Columns[columna] != null) grid.Columns[columna].Visible = false;
            }
            if (grid.Columns["UsuarioLogin"] != null) grid.Columns["UsuarioLogin"].HeaderText = "Usuario";
            if (grid.Columns["Estado"] != null) grid.Columns["Estado"].HeaderText = "Activo";
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void AplicarBusqueda()
        {
            try
            {
                string busqueda = txtBuscar.Text.Trim();

                listaUsuarios = string.IsNullOrWhiteSpace(busqueda)
                    ? listaUsuariosCompleta
                    : listaUsuariosCompleta
                        .Where(u =>
                            u.Nombre.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            u.Apellido.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            u.UsuarioLogin.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                grid.DataSource = null;
                grid.DataSource = listaUsuarios;

                foreach (string columna in new[] { "IdUsuario", "Password", "TelegramChatId" })
                {
                    if (grid.Columns[columna] != null) grid.Columns[columna].Visible = false;
                }
                if (grid.Columns["UsuarioLogin"] != null) grid.Columns["UsuarioLogin"].HeaderText = "Usuario";
                if (grid.Columns["Estado"] != null) grid.Columns["Estado"].HeaderText = "Activo";
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                lblContador.Text = $"Total: {listaUsuarios.Count} usuario(s)";
                lblSinDatos.Text = listaUsuarios.Count == 0 ? "No hay usuarios registrados." : "";
                lblSinDatos.Visible = listaUsuarios.Count == 0;
                lblSinDatos.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null)
            {
                return;
            }

            usuarioSeleccionado = grid.CurrentRow.DataBoundItem as Usuario;
            if (usuarioSeleccionado == null) return; // <-- agregar esta línea


            txtNombre.Text = usuarioSeleccionado.Nombre;
            txtApellido.Text = usuarioSeleccionado.Apellido;
            txtCorreo.Text = usuarioSeleccionado.Correo;
            txtUsuarioLogin.Text = usuarioSeleccionado.UsuarioLogin;
            cboRol.SelectedItem = usuarioSeleccionado.Rol;
            chkEstado.Checked = usuarioSeleccionado.Estado;

            // El password nunca se edita directo (es un hash) — se deshabilita
            // y se usa el botón ResetPassword para cambiarlo.
            txtPassword.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmarPassword.Text = string.Empty;
            btnResetPassword.Enabled = true;

            lblTelegramStatus.Text = usuarioSeleccionado.TelegramChatId.HasValue
                ? "Telegram: Vinculado"
                : "Telegram: No vinculado";
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
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

            usuarioSeleccionado = null;
            txtNombre.Clear();
            txtApellido.Clear();
            txtCorreo.Clear();
            txtUsuarioLogin.Clear();
            txtPassword.Clear();
            txtPassword.Enabled = true;
            cboRol.SelectedIndex = -1;
            chkEstado.Checked = true;
            lblTelegramStatus.Text = string.Empty;
            btnResetPassword.Enabled = false;
            txtConfirmarPassword.Clear();

            txtNombre.Focus();
        }
        private bool ValidarCampos()
        {
            errorProvider1.Clear();
            bool esValido = true;

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "Este campo es obligatorio.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                errorProvider1.SetError(txtApellido, "Este campo es obligatorio.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                errorProvider1.SetError(txtCorreo, "Este campo es obligatorio.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtUsuarioLogin.Text))
            {
                errorProvider1.SetError(txtUsuarioLogin, "Este campo es obligatorio.");
                esValido = false;
            }

            if (usuarioSeleccionado == null && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Debes indicar una contraseña.");
                esValido = false;
            }

            return esValido;
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                if (usuarioSeleccionado != null && usuarioSeleccionado.Rol == "Administrador" && !chkEstado.Checked)
                {
                    int adminsActivos = listaUsuarios.Count(u =>
                        u.Rol == "Administrador" && u.Estado && u.IdUsuario != usuarioSeleccionado.IdUsuario);

                    if (adminsActivos == 0)
                    {
                        MessageBox.Show("No se puede desactivar: es el único Administrador activo del sistema.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (cboRol.SelectedItem == null)
                {
                    MessageBox.Show("Selecciona un rol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(txtPassword.Text) || usuarioSeleccionado == null)
                {
                    if (txtPassword.Text != txtConfirmarPassword.Text)
                    {
                        MessageBox.Show("Las contraseñas no coinciden.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                bool esNuevo = usuarioSeleccionado == null;

                if (esNuevo)
                {
                    Usuario nuevo = new Usuario(
                        0, txtNombre.Text, txtApellido.Text, txtCorreo.Text,
                        txtUsuarioLogin.Text, txtPassword.Text, cboRol.SelectedItem.ToString(),
                        chkEstado.Checked
                    );
                    usuarioLN.InsertUsuario(nuevo);

                    auditoriaLN.Registrar(
                        usuarioActual.IdUsuario,
                        $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                        "Crear",
                        "Usuario",
                        null,
                        $"Usuario creado: {txtUsuarioLogin.Text} | Rol: {cboRol.SelectedItem}"
                    );
                }
                else
                {
                    string rolAnterior = usuarioSeleccionado.Rol;
                    bool estadoAnterior = usuarioSeleccionado.Estado;
                    bool cambioPassword = !string.IsNullOrWhiteSpace(txtPassword.Text);

                    usuarioSeleccionado.Nombre = txtNombre.Text;
                    usuarioSeleccionado.Apellido = txtApellido.Text;
                    usuarioSeleccionado.Correo = txtCorreo.Text;
                    usuarioSeleccionado.UsuarioLogin = txtUsuarioLogin.Text;
                    usuarioSeleccionado.Rol = cboRol.SelectedItem.ToString();
                    usuarioSeleccionado.Estado = chkEstado.Checked;

                    if (cambioPassword)
                    {
                        usuarioLN.CambiarPassword(usuarioSeleccionado, txtPassword.Text);
                    }
                    else
                    {
                        usuarioLN.UpdateUsuario(usuarioSeleccionado);
                    }

                    string detalle = $"Usuario: {usuarioSeleccionado.UsuarioLogin}";
                    if (rolAnterior != usuarioSeleccionado.Rol) detalle += $" | Rol: {rolAnterior} → {usuarioSeleccionado.Rol}";
                    if (estadoAnterior != usuarioSeleccionado.Estado) detalle += $" | Activo: {estadoAnterior} → {usuarioSeleccionado.Estado}";
                    if (cambioPassword) detalle += " | Contraseña cambiada";

                    auditoriaLN.Registrar(
                        usuarioActual.IdUsuario,
                        $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                        "Modificar",
                        "Usuario",
                        usuarioSeleccionado.IdUsuario,
                        detalle
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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("Selecciona un usuario primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (usuarioSeleccionado.IdUsuario == usuarioActual.IdUsuario)
            {
                MessageBox.Show("No puedes eliminar tu propia cuenta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (usuarioSeleccionado.Rol == "Administrador")
            {
                int adminsActivos = listaUsuarios.Count(u =>
                    u.Rol == "Administrador" && u.Estado && u.IdUsuario != usuarioSeleccionado.IdUsuario);

                if (adminsActivos == 0)
                {
                    MessageBox.Show("No se puede eliminar: es el único Administrador activo del sistema.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            DialogResult confirmacion = MessageBox.Show(
                $"¿Eliminar al usuario {usuarioSeleccionado.Nombre} {usuarioSeleccionado.Apellido}?\n\n" +
                "Si tiene incidencias asignadas como técnico, no se podrá eliminar — considera desactivarlo en vez de eliminarlo.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes) return;

            try
            {
                string usuarioEliminado = usuarioSeleccionado.UsuarioLogin;
                int idEliminado = usuarioSeleccionado.IdUsuario;

                usuarioLN.DeleteUsuario(usuarioSeleccionado);

                auditoriaLN.Registrar(
                    usuarioActual.IdUsuario,
                    $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                    "Eliminar",
                    "Usuario",
                    idEliminado,
                    $"Usuario eliminado: {usuarioEliminado}"
                );

                CargarGrid();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (usuarioSeleccionado == null) return;

            DialogResult confirmacion = MessageBox.Show(
                $"¿Generar una nueva contraseña temporal para {usuarioSeleccionado.UsuarioLogin}?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes) return;

          

            try
            {
                string passwordTemporal = usuarioLN.GenerarPasswordTemporal(usuarioSeleccionado);

                auditoriaLN.Registrar(
                    usuarioActual.IdUsuario,
                    $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                    "Modificar",
                    "Usuario",
                    usuarioSeleccionado.IdUsuario,
                    $"Contraseña temporal generada para: {usuarioSeleccionado.UsuarioLogin}"
                );

                MessageBox.Show(
                    $"Contraseña temporal generada:\n\n{passwordTemporal}\n\n" +
                    "Comunícasela al usuario. No se volverá a mostrar.",
                    "Contraseña Temporal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panelFormulario_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarBusqueda();

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void FrmUsuarios_KeyDown(object sender, KeyEventArgs e)
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
                AplicarBusqueda();
                e.Handled = true;
            }
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] pdf = IncidenciaReportes.GenerarPdfListadoUsuarios(listaUsuarios);
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
                byte[] excel = IncidenciaReportes.GenerarExcelListadoUsuarios(listaUsuarios);
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
                dialogo.FileName = $"Usuarios_{DateTime.Now:yyyyMMdd_HHmm}.{extension}";

                if (dialogo.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(dialogo.FileName, contenido);
                    MessageBox.Show("Exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        }
}
