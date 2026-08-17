using Datos.Gestion_de_Datos;
using System;
using System.Collections.Generic;
using Auditoria = Entidades.Gestion_de_Entidades.Auditoria;

namespace Logica.Gestion_de_Logica
{
    public class AuditoriaLN
    {
        /// <summary>
        /// Registra una acción en la bitácora. Es "best effort": si falla el registro
        /// de auditoría, NO debe romper la operación real (guardar/eliminar) que la
        /// generó — por eso el catch está vacío a propósito.
        /// </summary>
        public void Registrar(int? idUsuario, string nombreUsuario, string accion, string entidad, int? entidadId, string detalle)
        {
            try
            {
                AuditoriaCD.Insertar(idUsuario, nombreUsuario, accion, entidad, entidadId, detalle);
            }
            catch
            {
            }
        }

        public List<Auditoria> ShowAuditoria()
        {
            List<Auditoria> lista = new List<Auditoria>();
            try
            {
                var auxLista = AuditoriaCD.Listar();
                foreach (var obj in auxLista)
                {
                    lista.Add(new Auditoria(obj.IdAuditoria, obj.Fecha, obj.IdUsuario, obj.NombreUsuario, obj.Accion, obj.Entidad, obj.EntidadId, obj.Detalle));
                }
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al mostrar auditoría", ex);
            }
            return lista;
        }
    }
}