using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class FrmAcercaDe : Form
    {
        public FrmAcercaDe()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://t.me/appb2027_incidencias_bot");
            }
            catch
            {
                MessageBox.Show("No se pudo abrir Telegram. Búscalo manualmente: @appb2027_incidencias_bot",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
