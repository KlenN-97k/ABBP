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
    public partial class FrmPrincipal : Form
    {
        private Usuario _usuarioActual;

        public FrmPrincipal(Usuario usuario)
        {
            InitializeComponent();

            toolTip1.SetToolTip(btnIncidencias, "Gestionar incidencias reportadas");
            toolTip1.SetToolTip(btnUsuarios, "Administrar usuarios del sistema");
            toolTip1.SetToolTip(btnAreas, "Administrar áreas de la organización");
            toolTip1.SetToolTip(btnDashboard, "Ver métricas y gráficos del sistema");
            toolTip1.SetToolTip(btnReportes, "Exportar reportes en PDF o Excel");
            toolTip1.SetToolTip(btnGuias, "Consultar guías de ayuda");
            toolTip1.SetToolTip(btnAuditoria, "Ver bitácora de auditoría del sistema");
            toolTip1.SetToolTip(btnCerrarSesion, "Cerrar la sesión actual");
            _usuarioActual = usuario;
            ActualizarFotoSidebar();
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            lblUsuarioSidebar.Text = $"{_usuarioActual.Nombre} {_usuarioActual.Apellido}\n({_usuarioActual.Rol})";
           


            if (_usuarioActual.Rol == "Usuario")
            {
                DeshabilitarBoton(btnUsuarios);
                DeshabilitarBoton(btnAreas);
                DeshabilitarBoton(btnReportes);
                DeshabilitarBoton(btnAuditoria);
            }
            else if (_usuarioActual.Rol == "Técnico")
            {
                DeshabilitarBoton(btnUsuarios);
                DeshabilitarBoton(btnAreas);
                DeshabilitarBoton(btnAuditoria);
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
        private void DeshabilitarBoton(Button boton)
        {
            boton.Enabled = false;
            boton.BackColor = Color.FromArgb(50, 65, 85);   // azul marino más apagado
            boton.ForeColor = Color.FromArgb(140, 150, 160); // texto gris tenue
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
            AbrirHijo(new FrmIncidencias(_usuarioActual));

        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmUsuarios(_usuarioActual));

        }

        private void btnAreas_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmAreas(_usuarioActual));

        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmManual(_usuarioActual));
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmDashboard());
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

        private void btnAuditoria_Click(object sender, EventArgs e)
        {
            AbrirHijo(new FrmAuditoria());

        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult confirmacion = MessageBox.Show(
                    "¿Seguro que deseas salir del sistema?",
                    "Confirmar salida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void btnAcercaDe_Click(object sender, EventArgs e)
        {
            using (FrmAcercaDe dlg = new FrmAcercaDe())
            {
                dlg.ShowDialog();
            }
        }
        private void ActualizarFotoSidebar()
        {
            if (_usuarioActual.FotoPerfil != null && _usuarioActual.FotoPerfil.Length > 0)
            {
                using (var ms = new MemoryStream(_usuarioActual.FotoPerfil))
                using (Image original = Image.FromStream(ms))
                {
                    btnMiPerfil.Image = new Bitmap(original, new Size(55, 55)); // ajusta al tamaño real de tu botón
                }
            }
            else
            {
                btnMiPerfil.Image = null;
            }
        }

        private void btnMiPerfil_Click(object sender, EventArgs e)
        {
            FrmPerfil frm = new FrmPerfil(_usuarioActual);
            frm.FotoActualizada += (s, args) => ActualizarFotoSidebar();
            AbrirHijo(frm);

        }
    }
}
