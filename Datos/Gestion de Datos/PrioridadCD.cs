using Datos.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Gestion_de_Datos
{

    public class PrioridadCD
    {
        public static List<sp_Prioridades_ListarResult> ListarPrioridades()
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    return DB.sp_Prioridades_Listar().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al ejecutar el procedimiento Listar prioridades", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void InsertarPrioridad(Entidades.Gestion_de_Entidades.Prioridad oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Prioridades_Insertar(oc.Nombre);
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Prioridades", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void ModificarPrioridad(Entidades.Gestion_de_Entidades.Prioridad oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Prioridades_Modificar(oc.IdPrioridad, oc.Nombre);
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Prioridades", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void EliminarPrioridad(Entidades.Gestion_de_Entidades.Prioridad oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Prioridades_Eliminar(oc.IdPrioridad);
                    DB.SubmitChanges();
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al eliminar en la tabla Prioridades"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Prioridades", ex);
            }
            finally
            {
                DB = null;
            }
        }

    }
}
