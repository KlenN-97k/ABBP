using Dapper;
using Entidades.Gestion_de_Entidades;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos.Gestion_de_Datos
{
    public class AreaCD
    {
        public static List<Area> ListarAreas()
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    return conexion.Query<Area>("SELECT IdArea, NombreArea FROM Areas ORDER BY NombreArea;").ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al listar áreas", ex);
            }
        }

        public static void InsertarArea(Area oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("INSERT INTO Areas (NombreArea) VALUES (@NombreArea);", oc);
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Areas", ex);
            }
        }

        public static void ModificarArea(Area oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("UPDATE Areas SET NombreArea = @NombreArea WHERE IdArea = @IdArea;", oc);
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Areas", ex);
            }
        }

        public static void EliminarArea(Area oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("DELETE FROM Areas WHERE IdArea = @IdArea;", new { oc.IdArea });
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Areas: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Areas", ex);
            }
        }
    }
}