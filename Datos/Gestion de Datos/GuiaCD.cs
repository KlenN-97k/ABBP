using Datos.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Gestion_de_Datos
{
    public class GuiaCD
    {
        public static List<sp_Guias_ListarResult> ListarGuias()
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    return DB.sp_Guias_Listar().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al ejecutar el procedimiento Listar guías", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void InsertarGuia(Entidades.Gestion_de_Entidades.Guia oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Guias_Insertar(oc.Titulo, oc.Problema, oc.Solucion);
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Guias", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void ModificarGuia(Entidades.Gestion_de_Entidades.Guia oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Guias_Modificar(oc.IdGuia, oc.Titulo, oc.Problema, oc.Solucion);
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Guias", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void EliminarGuia(Entidades.Gestion_de_Entidades.Guia oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Guias_Eliminar(oc.IdGuia);
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Guias", ex);
            }
            finally
            {
                DB = null;
            }
        }
    }

}
