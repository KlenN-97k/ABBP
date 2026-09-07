using Dapper;
using Entidades.Gestion_de_Entidades;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos.Gestion_de_Datos
{
    public class GuiaCD
    {
        public static List<Guia> ListarGuias()
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    return conexion.Query<Guia>("SELECT IdGuia, Titulo, Problema, Solucion FROM Guias ORDER BY Titulo;").ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al listar guías", ex);
            }
        }

        public static void InsertarGuia(Guia oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("INSERT INTO Guias (Titulo, Problema, Solucion) VALUES (@Titulo, @Problema, @Solucion);", oc);
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Guias", ex);
            }
        }

        public static void ModificarGuia(Guia oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("UPDATE Guias SET Titulo = @Titulo, Problema = @Problema, Solucion = @Solucion WHERE IdGuia = @IdGuia;", oc);
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Guias", ex);
            }
        }

        public static void EliminarGuia(Guia oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("DELETE FROM Guias WHERE IdGuia = @IdGuia;", new { oc.IdGuia });
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Guias", ex);
            }
        }
    }
}