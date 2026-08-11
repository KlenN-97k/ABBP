using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reportes
{
    public class MetricasIncidencias
    {
        private int total;
        private Dictionary<string, int> porEstado;
        private Dictionary<string, int> porPrioridad;
        private Dictionary<string, int> porArea;
        private double? tiempoPromedioResolucionHoras;

        public MetricasIncidencias()
        {
            PorEstado = new Dictionary<string, int>();
            PorPrioridad = new Dictionary<string, int>();
            PorArea = new Dictionary<string, int>();
        }

        public int Total { get => total; set => total = value; }
        public Dictionary<string, int> PorEstado { get => porEstado; set => porEstado = value; }
        public Dictionary<string, int> PorPrioridad { get => porPrioridad; set => porPrioridad = value; }
        public Dictionary<string, int> PorArea { get => porArea; set => porArea = value; }
        public double? TiempoPromedioResolucionHoras { get => tiempoPromedioResolucionHoras; set => tiempoPromedioResolucionHoras = value; }
    }
}
