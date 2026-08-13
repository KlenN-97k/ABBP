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
    public partial class FrmPrincipal : Form
    {
        private Usuario _usuarioActual;

        public FrmPrincipal(Usuario usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            lblUsuarioSidebar.Text = $"{_usuarioActual.Nombre} {_usuarioActual.Apellido}\n({_usuarioActual.Rol})";

            if (_usuarioActual.Rol == "Usuario")
            {
                btnUsuarios.Visible = false;
                btnAreas.Visible = false;
                btnReportes.Visible = false;
            }
            else if (_usuarioActual.Rol == "Técnico")
            {
                btnUsuarios.Visible = false;
                btnAreas.Visible = false;
            }

            foreach (Control control in this.Controls)
            {
                if (control is MdiClient mdiClient)
                {
                    mdiClient.BackColor = Color.FromArgb(21, 50, 80);
                    mdiClient.BackgroundImage = Properties.Resources.fondo4;
                    mdiClient.BackgroundImageLayout = ImageLayout.Stretch;
                    mdiClient.Invalidate(); // fuerza a repintar con el tamaño ya maximizado
                }
            }

        }
        private void AbrirHijo(Form formularioNuevo)
        {
            foreach (Form hijoAbierto in this.MdiChildren.ToArray())
            {
                hijoAbierto.Close();
            }

            formularioNuevo.MdiParent = this;
            formularioNuevo.Show();
            formularioNuevo.WindowState = FormWindowState.Maximized;
        }
        private void btnIncidencias_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmIncidencias());

        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmUsuarios());

        }

        private void btnAreas_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmAreas());

        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmDashboard());
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {

        }

        private void btnGuias_Click(object sender, EventArgs e)
        {
          AbrirHijo(new FrmGuias());
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult confirmacion = MessageBox.Show(
                "¿Seguro que deseas cerrar sesión?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                this.Hide();
                FrmLogin login = new FrmLogin();
                login.FormClosed += (s, e2) => this.Close();
                login.Show();
            }

            }

        private void FrmPrincipal_Resize(object sender, EventArgs e)
        {


        }

        private void panelSidebar_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
