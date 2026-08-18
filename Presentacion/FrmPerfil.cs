using Entidades.Gestion_de_Entidades;
using Logica.Gestion_de_Logica;
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
    public partial class FrmPerfil : Form
    {
        private readonly UsuarioLN usuarioLN = new UsuarioLN();
        private readonly AuditoriaLN auditoriaLN = new AuditoriaLN();
        private Usuario usuarioActual;

        public FrmPerfil(Usuario usuarioActual)
        {
            InitializeComponent();
            ConfigurarOjito(lblOjoActual, txtPasswordActual);
            ConfigurarOjito(lblOjoNueva, txtPasswordNueva);
            ConfigurarOjito(lblOjoConfirmar, txtConfirmarPasswordNueva);
            this.usuarioActual = usuarioActual;
            CargarDatos();
        }
        public event EventHandler FotoActualizada;
        private void CargarDatos()
        {
            txtNombre.Text = usuarioActual.Nombre;
            txtApellido.Text = usuarioActual.Apellido;
            txtCorreo.Text = usuarioActual.Correo;
            txtUsuarioLogin.Text = usuarioActual.UsuarioLogin;
            txtRol.Text = usuarioActual.Rol;

            lblTelegramStatus.Text = usuarioActual.TelegramChatId.HasValue
                ? "Telegram: Vinculado ✅"
                : "Telegram: No vinculado (usa /registrar en el Bot)";

            CargarFoto();
        }

        private void CargarFoto()
        {
            if (usuarioActual.FotoPerfil != null && usuarioActual.FotoPerfil.Length > 0)
            {
                using (var ms = new MemoryStream(usuarioActual.FotoPerfil))
                {
                    picFoto.Image = Image.FromStream(ms);
                }
            }
            else
            {
                picFoto.Image = null;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmpleado_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void btnGuardarPerfil_Click(object sender, EventArgs e)
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

            if (!esValido) return;

            try
            {
                usuarioActual.Nombre = txtNombre.Text;
                usuarioActual.Apellido = txtApellido.Text;
                usuarioActual.Correo = txtCorreo.Text;

                usuarioLN.UpdateUsuario(usuarioActual);

                auditoriaLN.Registrar(
                    usuarioActual.IdUsuario,
                    $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                    "Modificar",
                    "Usuario",
                    usuarioActual.IdUsuario,
                    "Actualizó su propio perfil"
                );

                MessageBox.Show(
                    "Perfil actualizado correctamente.\n\nSi tu nombre cambió, se reflejará completamente al volver a iniciar sesión.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void FrmPerfil_Load(object sender, EventArgs e)
        {

        }

        private void btnCambiarFoto_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog dialogo = new OpenFileDialog())
            {
                dialogo.Filter = "Imágenes|*.jpg;*.jpeg;*.png";

                if (dialogo.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (Image original = Image.FromFile(dialogo.FileName))
                    using (Bitmap redimensionada = new Bitmap(original, new Size(200, 200)))
                    using (var ms = new MemoryStream())
                    {
                        redimensionada.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] fotoBytes = ms.ToArray();

                        usuarioLN.ActualizarFotoPerfil(usuarioActual, fotoBytes);

                        auditoriaLN.Registrar(
                            usuarioActual.IdUsuario,
                            $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                            "Modificar",
                            "Usuario",
                            usuarioActual.IdUsuario,
                            "Actualizó su foto de perfil"
                        );

                        CargarFoto();
                        FotoActualizada?.Invoke(this, EventArgs.Empty); 

                        MessageBox.Show("Foto actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            }
        private void ConfigurarOjito(Label lblOjo, TextBox txtPassword)
        {
            lblOjo.Text = "👁";
            lblOjo.Cursor = Cursors.Hand;
            lblOjo.Click += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
                lblOjo.Text = txtPassword.UseSystemPasswordChar ? "👁" : "🙈";
            };
        }

        private void btnQuitarFoto_Click_1(object sender, EventArgs e)
        {
         
            DialogResult confirmacion = MessageBox.Show(
                "¿Quitar tu foto de perfil?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes) return;

            try
            {
                usuarioLN.ActualizarFotoPerfil(usuarioActual, null);
                CargarFoto();
                FotoActualizada?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Foto eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
    }

        private void btnCambiarPassword_Click_1(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtPasswordActual.Text))
            {
                errorProvider1.SetError(txtPasswordActual, "Ingresa tu contraseña actual.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPasswordNueva.Text) || txtPasswordNueva.Text.Length < 6)
            {
                errorProvider1.SetError(txtPasswordNueva, "Mínimo 6 caracteres.");
                return;
            }

            if (txtPasswordNueva.Text != txtConfirmarPasswordNueva.Text)
            {
                errorProvider1.SetError(txtConfirmarPasswordNueva, "Las contraseñas no coinciden.");
                return;
            }

            try
            {
                Usuario verificado = usuarioLN.Login(usuarioActual.UsuarioLogin, txtPasswordActual.Text);

                if (verificado == null)
                {
                    errorProvider1.SetError(txtPasswordActual, "Contraseña actual incorrecta.");
                    return;
                }

                usuarioLN.CambiarPassword(usuarioActual, txtPasswordNueva.Text);

                auditoriaLN.Registrar(
                    usuarioActual.IdUsuario,
                    $"{usuarioActual.Nombre} {usuarioActual.Apellido}",
                    "Modificar",
                    "Usuario",
                    usuarioActual.IdUsuario,
                    "Cambió su propia contraseña"
                );

                txtPasswordActual.Clear();
                txtPasswordNueva.Clear();
                txtConfirmarPasswordNueva.Clear();

                MessageBox.Show("Contraseña actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
