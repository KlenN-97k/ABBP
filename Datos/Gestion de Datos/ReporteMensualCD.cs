using Dapper;
using MySqlConnector;
using System;

namespace Datos.Gestion_de_Datos
{
    public static class ReporteMensualCD
    {
        public static bool YaFueEnviado(int anio, int mes)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = "SELECT COUNT(*) FROM ReportesMensualesEnviados WHERE Anio = @anio AND Mes = @mes;";
                    int count = conexion.ExecuteScalar<int>(sql, new { anio, mes });
                    return count > 0;
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new DatosExcepciones("Error al verificar el reporte mensual: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al verificar el reporte mensual", ex);
            }
        }

        public static void RegistrarEnvio(int anio, int mes)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = "INSERT INTO ReportesMensualesEnviados (Anio, Mes, FechaEnvio) VALUES (@anio, @mes, NOW());";
                    conexion.Execute(sql, new { anio, mes });
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new DatosExcepciones("Error al registrar el envío del reporte mensual: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al registrar el envío del reporte mensual", ex);
            }
        }
    }
}