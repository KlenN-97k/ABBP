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
    public partial class FrmAuditoria : Form
    {
        private readonly AuditoriaLN auditoriaLN = new AuditoriaLN();

        public FrmAuditoria()
        {
            InitializeComponent();
            CargarGrid();

        }
        private void CargarGrid()
        {
            try
            {
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                var registros = auditoriaLN.ShowAuditoria();
                grid.DataSource = null;
                grid.DataSource = registros;

                if (grid.Columns["IdAuditoria"] != null) grid.Columns["IdAuditoria"].Visible = false;
                if (grid.Columns["IdUsuario"] != null) grid.Columns["IdUsuario"].Visible = false;
                if (grid.Columns["EntidadId"] != null) grid.Columns["EntidadId"].Visible = false;

                if (grid.Columns["Fecha"] != null) grid.Columns["Fecha"].HeaderText = "Fecha";
                if (grid.Columns["NombreUsuario"] != null) grid.Columns["NombreUsuario"].HeaderText = "Usuario";
                if (grid.Columns["Accion"] != null) grid.Columns["Accion"].HeaderText = "Acción";
                if (grid.Columns["Entidad"] != null) grid.Columns["Entidad"].HeaderText = "Entidad";
                if (grid.Columns["Detalle"] != null) grid.Columns["Detalle"].HeaderText = "Detalle";


                lblSinDatos.Text = registros.Count == 0 ? "No hay registros de auditoría." : "";
                lblSinDatos.Visible = registros.Count == 0;
                lblSinDatos.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarGrid();

        }
    }
}
