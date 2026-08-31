using Datos.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Gestion_de_Datos
{
    public static class SlaCD
    {
        public static bool YaEscalado(int idIncidencia)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    int count = DB.ExecuteQuery<int>(
                        "SELECT COUNT(*) FROM dbo.Incidencias WHERE IdIncidencia = {0} AND EscaladoSLA = 1",
                        idIncidencia).First();
                    return count > 0;
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al verificar el escalamiento SLA"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al verificar el escalamiento SLA", ex);
            }
        }

        public static void MarcarEscalado(int idIncidencia)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.ExecuteCommand(
                        "UPDATE dbo.Incidencias SET EscaladoSLA = 1 WHERE IdIncidencia = {0}",
                        idIncidencia);
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al marcar el escalamiento SLA"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al marcar el escalamiento SLA", ex);
            }
        }
    }

}
