using Entidades.Gestion_de_Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reportes;

namespace Presentacion
{
    public partial class FrmFiltroIncidencias : Form
    {

        public FiltroIncidencias Filtro { get; private set; }
        public string TicketBusqueda { get; private set; }

        public FrmFiltroIncidencias(List<Estado> estados, FiltroIncidencias filtroActual)
        {
            InitializeComponent();

            var opciones = new List<Estado> { new Estado(0, "(Todos)") };
            opciones.AddRange(estados);
            cboEstado.DataSource = opciones;
            cboEstado.DisplayMember = "Nombre";
            cboEstado.ValueMember = "IdEstado";

            cboEstado.SelectedValue = filtroActual.IdEstado ?? 0;

            if (filtroActual.FechaDesde.HasValue && filtroActual.FechaHasta.HasValue)
            {
                chkFecha.Checked = true;
                dtpDesde.Value = filtroActual.FechaDesde.Value;
                dtpHasta.Value = filtroActual.FechaHasta.Value;
            }

        }

        private void FrmFiltroIncidencias_Load(object sender, EventArgs e)
        {

        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            Filtro = new FiltroIncidencias();

            if (cboEstado.SelectedValue != null && (int)cboEstado.SelectedValue != 0)
            {
                Filtro.IdEstado = (int)cboEstado.SelectedValue;
            }

            if (chkFecha.Checked)
            {
                Filtro.FechaDesde = dtpDesde.Value.Date;
                Filtro.FechaHasta = dtpHasta.Value.Date;
            }

    

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Filtro = new FiltroIncidencias();
            TicketBusqueda = string.Empty;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtTicket_TextChanged(object sender, EventArgs e)
        {
            Filtro = new FiltroIncidencias();

            if (cboEstado.SelectedValue != null && (int)cboEstado.SelectedValue != 0)
            {
                Filtro.IdEstado = (int)cboEstado.SelectedValue;
            }

            if (chkFecha.Checked)
            {
                Filtro.FechaDesde = dtpDesde.Value.Date;
                Filtro.FechaHasta = dtpHasta.Value.Date;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
