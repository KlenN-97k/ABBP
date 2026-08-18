using Entidades.Gestion_de_Entidades;
using Logica.Gestion_de_Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class FrmLogin : Form
    {
        private UsuarioLN _usuarioLN = new UsuarioLN();

        public FrmLogin()
        {
            InitializeComponent();
            _usuarioLN = new UsuarioLN();
            this.btnIngresar.Location = new System.Drawing.Point(100, 200);
            txtPassword.UseSystemPasswordChar = !passwordVisible;
            toolTip1.SetToolTip(btnIngresar, "Iniciar sesión en el sistema");

        }


        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string user = txtUsuario.Text.Trim();
            string pass = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Por favor, ingrese usuario y contraseña.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Obtenemos todos los usuarios y filtramos por credenciales
                Usuario usuarioValido = _usuarioLN.Login(user, pass);

                if (usuarioValido != null)
                {
                    // Abrimos el formulario principal pasándole los datos del usuario logueado
                    FrmPrincipal frmPrincipal = new FrmPrincipal(usuarioValido);
                    frmPrincipal.Show();
                    this.Hide(); // Ocultamos el Login
                }
                else
                {
                    MessageBox.Show("Credenciales incorrectas o usuario inactivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error); ex.ToString();
            }

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(
    "Contacta al Administrador del sistema para restablecer tu contraseña.",
    "Recuperar contraseña",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information);
        }
        private bool passwordVisible = false;

        private void lblVerPassword_Click(object sender, EventArgs e)
        {
            passwordVisible = !passwordVisible;
            txtPassword.UseSystemPasswordChar = !passwordVisible;
            lblVerPassword.Text = passwordVisible ? "🙈" : "👁";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
