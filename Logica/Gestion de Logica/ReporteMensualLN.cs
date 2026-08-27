using Datos.Gestion_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Gestion_de_Logica
{
    public class ReporteMensualLN
    {
        public bool YaFueEnviado(int anio, int mes)
        {
            try { return ReporteMensualCD.YaFueEnviado(anio, mes); }
            catch (Exception ex) { throw new LogicaExcepciones("Error al verificar si el reporte mensual ya fue enviado", ex); }
        }

        public void RegistrarEnvio(int anio, int mes)
        {
            try { ReporteMensualCD.RegistrarEnvio(anio, mes); }
            catch (Exception ex) { throw new LogicaExcepciones("Error al registrar el envío del reporte mensual", ex); }
        }
    }
}