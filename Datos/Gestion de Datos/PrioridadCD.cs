using Dapper;
using Entidades.Gestion_de_Entidades;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos.Gestion_de_Datos
{
    public class PrioridadCD
    {
        public static List<Prioridad> ListarPrioridades()
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    return conexion.Query<Prioridad>("SELECT IdPrioridad, Nombre FROM Prioridades ORDER BY IdPrioridad;").ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al listar prioridades", ex);
            }
        }

        public static void InsertarPrioridad(Prioridad oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("INSERT INTO Prioridades (Nombre) VALUES (@Nombre);", oc);
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Prioridades", ex);
            }
        }

        public static void ModificarPrioridad(Prioridad oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("UPDATE Prioridades SET Nombre = @Nombre WHERE IdPrioridad = @IdPrioridad;", oc);
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Prioridades", ex);
            }
        }

        public static void EliminarPrioridad(Prioridad oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("DELETE FROM Prioridades WHERE IdPrioridad = @IdPrioridad;", new { oc.IdPrioridad });
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Prioridades: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Prioridades", ex);
            }
        }
    }
}