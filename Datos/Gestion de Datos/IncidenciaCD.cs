using Dapper;
using Entidades.Gestion_de_Entidades;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos.Gestion_de_Datos
{
    public class IncidenciaCD
    {
        public static List<Incidencia> ListarIncidencias()
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = @"
                        SELECT i.IdIncidencia, i.NumeroTicket, i.Fecha, i.Empleado, i.IdArea,
                               a.NombreArea, i.TipoIncidencia, i.Descripcion, i.IdPrioridad,
                               p.Nombre AS NombrePrioridad, i.IdEstado, e.Nombre AS NombreEstado,
                               i.IdTecnicoAsignado, CONCAT(u.Nombre, ' ', u.Apellido) AS TecnicoAsignado,
                               i.FechaSolucion, i.Observaciones, i.FilaVersion
                        FROM Incidencias i
                        INNER JOIN Areas a ON i.IdArea = a.IdArea
                        INNER JOIN Prioridades p ON i.IdPrioridad = p.IdPrioridad
                        INNER JOIN Estados e ON i.IdEstado = e.IdEstado
                        LEFT JOIN Usuarios u ON i.IdTecnicoAsignado = u.IdUsuario
                        ORDER BY i.Fecha DESC;";

                    return conexion.Query<Incidencia>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al listar incidencias", ex);
            }
        }

        public static int InsertarIncidencia(Incidencia oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Open();

                    string sqlInsert = @"
                        INSERT INTO Incidencias
                            (Empleado, IdArea, TipoIncidencia, Descripcion, IdPrioridad, IdEstado, IdTecnicoAsignado, Observaciones)
                        VALUES
                            (@Empleado, @IdArea, @TipoIncidencia, @Descripcion, @IdPrioridad, @IdEstado, @IdTecnicoAsignado, @Observaciones);";

                    conexion.Execute(sqlInsert, oc);

                    int nuevoId = conexion.ExecuteScalar<int>("SELECT LAST_INSERT_ID();");

                    // MySQL no permite que una columna calculada dependa de AUTO_INCREMENT,
                    // así que el NumeroTicket se arma aquí, justo después del insert.
                    conexion.Execute(
                        "UPDATE Incidencias SET NumeroTicket = CONCAT('Solicitud-', LPAD(@Id, 5, '0')) WHERE IdIncidencia = @Id;",
                        new { Id = nuevoId });

                    return nuevoId;
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Incidencias: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Incidencias", ex);
            }
        }

        public static bool ModificarIncidencia(Incidencia oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = @"
                        UPDATE Incidencias
                        SET Empleado = @Empleado,
                            IdArea = @IdArea,
                            TipoIncidencia = @TipoIncidencia,
                            Descripcion = @Descripcion,
                            IdPrioridad = @IdPrioridad,
                            IdEstado = @IdEstado,
                            IdTecnicoAsignado = @IdTecnicoAsignado,
                            FechaSolucion = @FechaSolucion,
                            Observaciones = @Observaciones
                        WHERE IdIncidencia = @IdIncidencia
                          AND FilaVersion = @FilaVersion;";

                    int filasAfectadas = conexion.Execute(sql, oc);
                    return filasAfectadas > 0;
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Incidencias: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Incidencias", ex);
            }
        }

        public static void EliminarIncidencia(Incidencia oe)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("DELETE FROM Incidencias WHERE IdIncidencia = @IdIncidencia;", new { oe.IdIncidencia });
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Incidencias", ex);
            }
        }

        public static bool AsignarTecnicoTelegram(int idIncidencia, int idTecnico, int idEstadoEnProceso)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = @"
                        UPDATE Incidencias
                        SET IdTecnicoAsignado = @idTecnico, IdEstado = @idEstadoEnProceso
                        WHERE IdIncidencia = @idIncidencia AND IdTecnicoAsignado IS NULL;";

                    int filas = conexion.Execute(sql, new { idIncidencia, idTecnico, idEstadoEnProceso });
                    return filas > 0;
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al ejecutar la asignación de técnico por Telegram", ex);
            }
        }

        public static bool ActualizarEstadoTelegram(int idIncidencia, int idTecnico, int idNuevoEstado, string observacion)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = @"
                        UPDATE Incidencias
                        SET IdEstado = @idNuevoEstado,
                            Observaciones = @observacion,
                            FechaSolucion = NOW()
                        WHERE IdIncidencia = @idIncidencia AND IdTecnicoAsignado = @idTecnico;";

                    int filas = conexion.Execute(sql, new { idIncidencia, idTecnico, idNuevoEstado, observacion });
                    return filas > 0;
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al actualizar el estado por Telegram", ex);
            }
        }

        public static void RegistrarMensajeTelegram(int idIncidencia, long chatId, int messageId)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute(
                        "INSERT INTO TelegramMensajes (IdIncidencia, ChatId, MessageId) VALUES (@idIncidencia, @chatId, @messageId);",
                        new { idIncidencia, chatId, messageId });
                }
            }
            catch { /* Ignorar errores de bitácora, igual que antes */ }
        }

        public class MensajeTelegramDTO { public long ChatId { get; set; } public int MessageId { get; set; } }

        public static List<MensajeTelegramDTO> ObtenerMensajesTelegram(int idIncidencia)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    return conexion.Query<MensajeTelegramDTO>(
                        "SELECT ChatId, MessageId FROM TelegramMensajes WHERE IdIncidencia = @idIncidencia;",
                        new { idIncidencia }).ToList();
                }
            }
            catch { return new List<MensajeTelegramDTO>(); }
        }
    }
}