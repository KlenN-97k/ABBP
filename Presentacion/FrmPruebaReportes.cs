using Logica.Gestion_de_Logica;
using Reportes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class FrmPruebaReportes : Form
    {
        public FrmPruebaReportes()
        {
            InitializeComponent();
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            try
            {
                var incidencias = new IncidenciaLN().ShowIncidencia();
                byte[] pdf = IncidenciaReportes.GenerarPdfListado(incidencias);

                string ruta = Path.Combine(Path.GetTempPath(), "reporte_incidencias.pdf");
                File.WriteAllBytes(ruta, pdf);
                Process.Start(ruta);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var incidencias = new IncidenciaLN().ShowIncidencia();
                byte[] excel = IncidenciaReportes.GenerarExcelListado(incidencias);

                string ruta = Path.Combine(Path.GetTempPath(), "reporte_incidencias.xlsx");
                File.WriteAllBytes(ruta, excel);
                Process.Start(ruta);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var incidencias = new IncidenciaLN().ShowIncidencia();
                MetricasIncidencias metricas = IncidenciaReportes.CalcularMetricas(incidencias);

                var texto = new System.Text.StringBuilder();
                texto.AppendLine($"Total: {metricas.Total}");

                texto.AppendLine("\nPor Estado:");
                foreach (var kvp in metricas.PorEstado)
                    texto.AppendLine($"  {kvp.Key}: {kvp.Value}");

                texto.AppendLine("\nPor Prioridad:");
                foreach (var kvp in metricas.PorPrioridad)
                    texto.AppendLine($"  {kvp.Key}: {kvp.Value}");

                texto.AppendLine("\nPor Área:");
                foreach (var kvp in metricas.PorArea)
                    texto.AppendLine($"  {kvp.Key}: {kvp.Value}");

                texto.AppendLine($"\nTiempo promedio de resolución: " +
                    (metricas.TiempoPromedioResolucionHoras.HasValue
                        ? $"{metricas.TiempoPromedioResolucionHoras.Value:0.##} horas"
                        : "N/A (ninguna resuelta)"));

                MessageBox.Show(texto.ToString(), "Métricas de Incidencias");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var guias = new GuiaLN().ShowGuia();
                byte[] pdf = IncidenciaReportes.GenerarPdfListadoGuias(guias);

                string ruta = Path.Combine(Path.GetTempPath(), "reporte_guias.pdf");
                File.WriteAllBytes(ruta, pdf);
                Process.Start(ruta);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var guias = new GuiaLN().ShowGuia();
                byte[] excel = IncidenciaReportes.GenerarExcelListadoGuias(guias);

                string ruta = Path.Combine(Path.GetTempPath(), "reporte_guias.xlsx");
                File.WriteAllBytes(ruta, excel);
                Process.Start(ruta);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
