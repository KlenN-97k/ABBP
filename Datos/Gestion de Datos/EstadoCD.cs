using Datos.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Gestion_de_Datos
{
    public class EstadoCD
    {
        public static List<sp_Estados_ListarResult> ListarEstados()
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    return DB.sp_Estados_Listar().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al ejecutar el procedimiento Listar estados", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void InsertarEstado(Entidades.Gestion_de_Entidades.Estado oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Estados_Insertar(oc.Nombre);
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Estados", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void ModificarEstado(Entidades.Gestion_de_Entidades.Estado oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Estados_Modificar(oc.IdEstado, oc.Nombre);
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Estados", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void EliminarEstado(Entidades.Gestion_de_Entidades.Estado oe)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Estados_Eliminar(oe.IdEstado);
                    DB.SubmitChanges();
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al eliminar en la tabla Estados"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Estados", ex);
            }
            finally
            {
                DB = null;
            }
        }

    }
}
