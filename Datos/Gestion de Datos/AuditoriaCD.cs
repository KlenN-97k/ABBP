using Datos.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Gestion_de_Datos
{
    public class AuditoriaCD
    {
        public static void Insertar(int? idUsuario, string nombreUsuario, string accion, string entidad, int? entidadId, string detalle)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Auditoria_Insertar(idUsuario, nombreUsuario, accion, entidad, entidadId, detalle);
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Auditoria", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static List<sp_Auditoria_ListarResult> Listar()
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    return DB.sp_Auditoria_Listar().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al ejecutar el procedimiento Listar auditoría", ex);
            }
            finally
            {
                DB = null;
            }
        }
    }
    }
