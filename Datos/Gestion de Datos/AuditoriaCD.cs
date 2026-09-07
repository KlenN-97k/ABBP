using Dapper;
using Entidades.Gestion_de_Entidades;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos.Gestion_de_Datos
{
    public class AuditoriaCD
    {
        public static void Insertar(int? idUsuario, string nombreUsuario, string accion, string entidad, int? entidadId, string detalle)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = @"
                        INSERT INTO Auditoria (IdUsuario, NombreUsuario, Accion, Entidad, EntidadId, Detalle)
                        VALUES (@idUsuario, @nombreUsuario, @accion, @entidad, @entidadId, @detalle);";

                    conexion.Execute(sql, new { idUsuario, nombreUsuario, accion, entidad, entidadId, detalle });
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Auditoria", ex);
            }
        }

        public static List<Auditoria> Listar()
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = @"
                        SELECT IdAuditoria, Fecha, IdUsuario, NombreUsuario, Accion, Entidad, EntidadId, Detalle
                        FROM Auditoria
                        ORDER BY Fecha DESC;";

                    return conexion.Query<Auditoria>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al listar auditoría", ex);
            }
        }
    }
}