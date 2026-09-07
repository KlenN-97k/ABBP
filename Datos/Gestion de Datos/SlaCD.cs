using Dapper;
using MySqlConnector;
using System;

namespace Datos.Gestion_de_Datos
{
    public static class SlaCD
    {
        public static bool YaEscalado(int idIncidencia)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = "SELECT COUNT(*) FROM Incidencias WHERE IdIncidencia = @idIncidencia AND EscaladoSLA = 1;";
                    int count = conexion.ExecuteScalar<int>(sql, new { idIncidencia });
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al verificar el escalamiento SLA", ex);
            }
        }

        public static void MarcarEscalado(int idIncidencia)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("UPDATE Incidencias SET EscaladoSLA = 1 WHERE IdIncidencia = @idIncidencia;", new { idIncidencia });
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al marcar el escalamiento SLA", ex);
            }
        }
    }
}