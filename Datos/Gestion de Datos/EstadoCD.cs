using Dapper;
using Entidades.Gestion_de_Entidades;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos.Gestion_de_Datos
{
    public class EstadoCD
    {
        public static List<Estado> ListarEstados()
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    return conexion.Query<Estado>("SELECT IdEstado, Nombre FROM Estados ORDER BY IdEstado;").ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al listar estados", ex);
            }
        }

        public static void InsertarEstado(Estado oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("INSERT INTO Estados (Nombre) VALUES (@Nombre);", oc);
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Estados", ex);
            }
        }

        public static void ModificarEstado(Estado oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("UPDATE Estados SET Nombre = @Nombre WHERE IdEstado = @IdEstado;", oc);
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Estados", ex);
            }
        }

        public static void EliminarEstado(Estado oe)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("DELETE FROM Estados WHERE IdEstado = @IdEstado;", new { oe.IdEstado });
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Estados: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Estados", ex);
            }
        }
    }
}