using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Gestion_de_Entidades
{
    public class Auditoria
    {
        private int idAuditoria;
        private DateTime fecha;
        private int? idUsuario;
        private string nombreUsuario;
        private string accion;
        private string entidad;
        private int? entidadId;
        private string detalle;

        public Auditoria() { }

        public Auditoria(int idAuditoria, DateTime fecha, int? idUsuario, string nombreUsuario, string accion, string entidad, int? entidadId, string detalle)
        {
            this.IdAuditoria = idAuditoria;
            this.Fecha = fecha;
            this.IdUsuario = idUsuario;
            this.NombreUsuario = nombreUsuario;
            this.Accion = accion;
            this.Entidad = entidad;
            this.EntidadId = entidadId;
            this.Detalle = detalle;
        }

        public int IdAuditoria { get => idAuditoria; set => idAuditoria = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public int? IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
        public string Accion { get => accion; set => accion = value; }
        public string Entidad { get => entidad; set => entidad = value; }
        public int? EntidadId { get => entidadId; set => entidadId = value; }
        public string Detalle { get => detalle; set => detalle = value; }
    }
}
