using Datos.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Gestion_de_Datos
{
    public class AreaCD
    {
        public static List<sp_Areas_ListarResult> ListarAreas()
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    return DB.sp_Areas_Listar().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al ejecutar el procedimiento Listar áreas", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void InsertarArea(Entidades.Gestion_de_Entidades.Area oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Areas_Insertar(oc.NombreArea);
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Areas", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void ModificarArea(Entidades.Gestion_de_Entidades.Area oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Areas_Modificar(oc.IdArea, oc.NombreArea);
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Areas", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void EliminarArea(Entidades.Gestion_de_Entidades.Area oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Areas_Eliminar(oc.IdArea);
                    DB.SubmitChanges();
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al eliminar en la tabla Areas"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Areas", ex);
            }
            finally
            {
                DB = null;
            }
        }
    }

}
