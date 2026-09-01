using Datos.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Gestion_de_Datos
{
    public class IncidenciaCD
    {
        public static List<sp_Incidencias_ListarResult> ListarIncidencias()
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    return DB.sp_Incidencias_Listar().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al ejecutar el procedimiento Listar incidencias", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static int InsertarIncidencia(Entidades.Gestion_de_Entidades.Incidencia oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    return DB.sp_Incidencias_Insertar(
                        oc.Empleado,
                        oc.IdArea,
                        oc.TipoIncidencia,
                        oc.Descripcion,
                        oc.IdPrioridad,
                        oc.IdEstado,
                        oc.IdTecnicoAsignado,
                        oc.Observaciones
                    );
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al insertar en la tabla Incidencias"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Incidencias", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static bool ModificarIncidencia(Entidades.Gestion_de_Entidades.Incidencia oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    int filasAfectadas = 
                        DB.sp_Incidencias_Modificar(
                        oc.IdIncidencia,
                        oc.Empleado,
                        oc.IdArea,
                        oc.TipoIncidencia,
                        oc.Descripcion,
                        oc.IdPrioridad,
                        oc.IdEstado,
                        oc.IdTecnicoAsignado,
                        oc.FechaSolucion,
                        oc.Observaciones,
                        oc.FilaVersion
                    );
                    return filasAfectadas > 0;
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al modificar en la tabla Incidencias"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Incidencias", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void EliminarIncidencia(Entidades.Gestion_de_Entidades.Incidencia oe)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Incidencias_Eliminar(oe.IdIncidencia);
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Incidencias", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static bool AsignarTecnicoTelegram(int idIncidencia, int idTecnico, int idEstadoEnProceso)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    int? resultado = 0;
                    // Llamada directa al procedimiento generado por LINQ to SQL
                    DB.sp_Incidencias_AsignarTecnicoTelegram(idIncidencia, idTecnico, idEstadoEnProceso, ref resultado);
                    return resultado.HasValue && resultado.Value == 1;
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al ejecutar procedimiento de asignación por Telegram", ex);
            }
        }

        public static bool ActualizarEstadoTelegram(int idIncidencia, int idTecnico, int idNuevoEstado, string observacion)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    int? resultado = 0;
                    DB.sp_Incidencias_ActualizarEstadoTelegram(idIncidencia, idTecnico, idNuevoEstado, observacion, ref resultado);
                    return resultado.HasValue && resultado.Value == 1;
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al ejecutar procedimiento de actualización de estado por Telegram", ex);
            }
        }

        public static void RegistrarMensajeTelegram(int idIncidencia, long chatId, int messageId)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.ExecuteCommand("INSERT INTO TelegramMensajes (IdIncidencia, ChatId, MessageId) VALUES ({0}, {1}, {2})", idIncidencia, chatId, messageId);
                }
            }
            catch { /* Ignorar errores de bitácora */ }
        }

        public class MensajeTelegramDTO { public long ChatId { get; set; } public int MessageId { get; set; } }

        public static List<MensajeTelegramDTO> ObtenerMensajesTelegram(int idIncidencia)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    return DB.ExecuteQuery<MensajeTelegramDTO>("SELECT ChatId, MessageId FROM TelegramMensajes WHERE IdIncidencia = {0}", idIncidencia).ToList();
                }
            }
            catch { return new List<MensajeTelegramDTO>(); }
        }
    }
}
