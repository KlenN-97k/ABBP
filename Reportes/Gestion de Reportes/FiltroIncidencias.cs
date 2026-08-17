using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reportes
{
    public class FiltroIncidencias
    {
        private DateTime? fechaDesde;
        private DateTime? fechaHasta;
        private int? idArea;
        private int? idPrioridad;
        private int? idEstado;
        private int? idTecnicoAsignado;

        public DateTime? FechaDesde { get => fechaDesde; set => fechaDesde = value; }
        public DateTime? FechaHasta { get => fechaHasta; set => fechaHasta = value; }
        public int? IdArea { get => idArea; set => idArea = value; }
        public int? IdPrioridad { get => idPrioridad; set => idPrioridad = value; }
        public int? IdEstado { get => idEstado; set => idEstado = value; }
        public int? IdTecnicoAsignado { get => idTecnicoAsignado; set => idTecnicoAsignado = value; }
    }
}
