using System;
using System.Data.SqlClient;
using System.Linq;
using Datos.Base_de_Datos;

namespace Datos.Gestion_de_Datos
{
    public static class ReporteMensualCD
    {
        public static bool YaFueEnviado(int anio, int mes)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    int count = DB.ExecuteQuery<int>(
                        "SELECT COUNT(*) FROM dbo.ReportesMensualesEnviados WHERE Anio = {0} AND Mes = {1}",
                        anio, mes).First();
                    return count > 0;
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al verificar el reporte mensual"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al verificar el reporte mensual", ex);
            }
        }

        public static void RegistrarEnvio(int anio, int mes)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.ExecuteCommand(
                        "INSERT INTO dbo.ReportesMensualesEnviados (Anio, Mes, FechaEnvio) VALUES ({0}, {1}, GETDATE())",
                        anio, mes);
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al registrar el reporte mensual"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al registrar el reporte mensual", ex);
            }
        }
    }
}