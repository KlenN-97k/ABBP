using Datos.Gestion_de_Datos;
using Entidades.Gestion_de_Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Gestion_de_Logica
{
    public class SlaLN
    {
        private static readonly Dictionary<string, TimeSpan> LimitesPorPrioridad = new Dictionary<string, TimeSpan>
        {
            { "Alta", TimeSpan.FromHours(4) },
            { "Media", TimeSpan.FromHours(24) },
            { "Baja", TimeSpan.FromHours(72) }
        };

        public List<Incidencia> ObtenerVencidasSinEscalar(List<Incidencia> pendientes)
        {
            var vencidas = new List<Incidencia>();

            foreach (var inc in pendientes)
            {
                if (!LimitesPorPrioridad.TryGetValue(inc.NombrePrioridad ?? "", out TimeSpan limite))
                    continue;

                if (DateTime.Now - inc.Fecha < limite)
                    continue;

                try
                {
                    if (!SlaCD.YaEscalado(inc.IdIncidencia))
                        vencidas.Add(inc);
                }
                catch (Exception ex)
                {
                    throw new LogicaExcepciones("Error al verificar el SLA de la incidencia " + inc.NumeroTicket, ex);
                }
            }

            return vencidas;
        }

        public void MarcarEscalado(int idIncidencia)
        {
            try { SlaCD.MarcarEscalado(idIncidencia); }
            catch (Exception ex) { throw new LogicaExcepciones("Error al marcar el escalamiento SLA", ex); }
        }
    }

}
