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
            // Configuramos el formulario para que contenga a los demás
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
        }



        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            // Mostrar los datos del usuario en la barra de estado inferior
            lblUsuarioStatus.Text = $"Usuario: {_usuarioActual.Nombre} {_usuarioActual.Apellido} | Rol: {_usuarioActual.Rol}";

            // ==========================================
            // CONTROL DE ACCESOS (ROLES)
            // ==========================================
            if (_usuarioActual.Rol == "Usuario")
            {
                // Un usuario normal no puede administrar el sistema ni ver reportes globales
                menuMantemientos.Visible = false;
                menuReportes.Visible = false;
            }
            else if (_usuarioActual.Rol == "Técnico")
            {
                // Un técnico atiende tickets y reportes, pero no crea Usuarios ni Áreas
                menuMantemientos.Visible = false;
            }
            // Si es 'Administrador', no entra a los IF y puede ver absolutamente todo.

        }

        private void mToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuIncidenciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Al abrir el formulario de incidencias, le pasamos el usuario actual
      
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void menuUsuarios_Click(object sender, EventArgs e)
        {
            FrmUsuarios frmUsuarios = new FrmUsuarios();
            frmUsuarios.MdiParent = this;
            frmUsuarios.Show();
        }

        private void menuAreas_Click(object sender, EventArgs e)
        {
            FrmAreas frmAreas = new FrmAreas();
            frmAreas.MdiParent = this;
            frmAreas.Show();
        }

        private void menuReportes_Click(object sender, EventArgs e)
        {
            // FrmReportes frmReportes = new FrmReportes();
            // frmReportes.MdiParent = this;
            // frmReportes.Show();
            MessageBox.Show("Módulo de Reportes en construcción...", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuGuias_Click(object sender, EventArgs e)
        {
            // FrmGuias frmGuias = new FrmGuias();
            // frmGuias.MdiParent = this;
            // frmGuias.Show();
            MessageBox.Show("Módulo de Guías en construcción...", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
