using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Gestion_de_Entidades
{
    public class Incidencia
    {
        private int idIncidencia;
        private string numeroTicket;
        private DateTime fecha;
        private string empleado;
        private int idArea;
        private string tipoIncidencia;
        private string descripcion;
        private int idPrioridad;
        private int idEstado;
        private int? idTecnicoAsignado;
        private DateTime? fechaSolucion;
        private string observaciones;

        // Campos de solo lectura para visualización. No se guardan en la tabla Incidencias:
        // vienen de los JOIN que hace sp_Incidencias_Listar (Areas, Prioridades, Estados, Usuarios)
        // para que Presentación pueda mostrar nombres en vez de IDs sin tener que hacer
        // lookups adicionales. Se dejan fuera del constructor porque Insertar/Modificar no los usan.
        private string nombreArea;
        private string nombrePrioridad;
        private string nombreEstado;
        private string tecnicoAsignado;

        // Control de concurrencia optimista: viene del ROWVERSION de SQL Server.
        // Solo se llena al listar (ShowIncidencia); se usa en Modificar para detectar
        // si otra persona cambió la incidencia mientras la teníamos abierta.
        private byte[] filaVersion;

        public Incidencia()
        {
        }

        public Incidencia(int idIncidencia, string numeroTicket, DateTime fecha, string empleado, int idArea, string tipoIncidencia, string descripcion, int idPrioridad, int idEstado, int? idTecnicoAsignado, DateTime? fechaSolucion, string observaciones)
        {
            this.IdIncidencia = idIncidencia;
            this.NumeroTicket = numeroTicket;
            this.Fecha = fecha;
            this.Empleado = empleado;
            this.IdArea = idArea;
            this.TipoIncidencia = tipoIncidencia;
            this.Descripcion = descripcion;
            this.IdPrioridad = idPrioridad;
            this.IdEstado = idEstado;
            this.IdTecnicoAsignado = idTecnicoAsignado;
            this.FechaSolucion = fechaSolucion;
            this.Observaciones = observaciones;
        }

        public int IdIncidencia { get => idIncidencia; set => idIncidencia = value; }
        public string NumeroTicket { get => numeroTicket; set => numeroTicket = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public string Empleado { get => empleado; set => empleado = value; }
        public int IdArea { get => idArea; set => idArea = value; }
        public string TipoIncidencia { get => tipoIncidencia; set => tipoIncidencia = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public int IdPrioridad { get => idPrioridad; set => idPrioridad = value; }
        public int IdEstado { get => idEstado; set => idEstado = value; }
        public int? IdTecnicoAsignado { get => idTecnicoAsignado; set => idTecnicoAsignado = value; }
        public DateTime? FechaSolucion { get => fechaSolucion; set => fechaSolucion = value; }
        public string Observaciones { get => observaciones; set => observaciones = value; }

        // Solo se llenan al listar (ShowIncidencia); quedan en null al crear una incidencia nueva.
        public string NombreArea { get => nombreArea; set => nombreArea = value; }
        public string NombrePrioridad { get => nombrePrioridad; set => nombrePrioridad = value; }
        public string NombreEstado { get => nombreEstado; set => nombreEstado = value; }
        public string TecnicoAsignado { get => tecnicoAsignado; set => tecnicoAsignado = value; }

        public byte[] FilaVersion { get => filaVersion; set => filaVersion = value; }
    }

}