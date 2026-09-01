using Datos.Base_de_Datos;
using Datos.Gestion_de_Datos;
using Entidades.Gestion_de_Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Incidencia = Entidades.Gestion_de_Entidades.Incidencia;

namespace Logica.Gestion_de_Logica
{
    public class IncidenciaLN
    {
        /* Nombres oficiales del catálogo Estados. 
         * Se resuelven en tiempo de ejecución para no depender del orden de los INSERT iniciales. */
        private const string ESTADO_PENDIENTE = "Pendiente";
        private const string ESTADO_EN_PROCESO = "En Proceso"; // Agregado para el bot
        private const string ESTADO_RESUELTO = "Resuelto";
        private const string ESTADO_CERRADO = "Cerrado";

        public List<Incidencia> ShowIncidencia()
        {
            List<Incidencia> lista = new List<Incidencia>();
            Incidencia oc;
            try
            {
                List<sp_Incidencias_ListarResult> auxLista = IncidenciaCD.ListarIncidencias();

                foreach (sp_Incidencias_ListarResult obj in auxLista)
                {
                    oc = new Incidencia(
                        obj.IdIncidencia,
                        obj.NumeroTicket,
                        obj.Fecha,
                        obj.Empleado,
                        obj.IdArea,
                        obj.TipoIncidencia,
                        obj.Descripcion,
                        obj.IdPrioridad,
                        obj.IdEstado,
                        obj.IdTecnicoAsignado,
                        obj.FechaSolucion,
                        obj.Observaciones
                    );

                    oc.NombreArea = obj.NombreArea;
                    oc.NombrePrioridad = obj.NombrePrioridad;
                    oc.NombreEstado = obj.NombreEstado;
                    oc.TecnicoAsignado = obj.TecnicoAsignado;

                    lista.Add(oc);
                }
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al mostrar incidencias", ex);
            }
            return lista;
        }

        public bool InsertIncidencia(Incidencia oe)
        {
            return InsertIncidencia(oe, out _);
        }

        public bool InsertIncidencia(Incidencia oe, out int idGenerado)
        {
            try
            {
                ValidarIncidencia(oe);
                oe.IdEstado = ObtenerIdEstadoPorNombre(ESTADO_PENDIENTE);
                idGenerado = IncidenciaCD.InsertarIncidencia(oe);
                return idGenerado > 0;
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al insertar incidencia en la BD", ex);
            }
        }

        public bool UpdateIncidencia(Incidencia oe)
        {
            try
            {
                ValidarIncidencia(oe);

                List<sp_Estados_ListarResult> estados = EstadoCD.ListarEstados();
                int idResuelto = ObtenerIdEstadoPorNombre(estados, ESTADO_RESUELTO);
                int idCerrado = ObtenerIdEstadoPorNombre(estados, ESTADO_CERRADO);
                bool esResueltaOCerrada = (oe.IdEstado == idResuelto || oe.IdEstado == idCerrado);

                if (esResueltaOCerrada && oe.IdTecnicoAsignado == null)
                {
                    throw new LogicaExcepciones("No se puede marcar la incidencia como Resuelto/Cerrado sin un técnico asignado.", null);
                }

                if (esResueltaOCerrada && oe.FechaSolucion == null)
                {
                    oe.FechaSolucion = DateTime.Now;
                }

                return IncidenciaCD.ModificarIncidencia(oe);
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al actualizar incidencia en la BD", ex);
            }
        }

        public bool DeleteIncidencia(Incidencia oe)
        {
            try
            {
                IncidenciaCD.EliminarIncidencia(oe);
                return true;
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al eliminar incidencia en la BD", ex);
            }
        }

        private int ObtenerIdEstadoPorNombre(string nombreEstado)
        {
            List<sp_Estados_ListarResult> estados = EstadoCD.ListarEstados();
            return ObtenerIdEstadoPorNombre(estados, nombreEstado);
        }

        private int ObtenerIdEstadoPorNombre(List<sp_Estados_ListarResult> estados, string nombreEstado)
        {
            sp_Estados_ListarResult estado = estados.FirstOrDefault(e => e.Nombre == nombreEstado);

            if (estado == null)
            {
                throw new LogicaExcepciones($"No se encontró el estado '{nombreEstado}' en el catálogo dbo.Estados.", null);
            }
            return estado.IdEstado;
        }

        private void ValidarIncidencia(Incidencia oe)
        {
            if (string.IsNullOrWhiteSpace(oe.Empleado)) throw new LogicaExcepciones("Debe indicar el nombre del empleado.", null);
            if (oe.Empleado.Length > 150) throw new LogicaExcepciones("El nombre no puede superar los 150 caracteres.", null);
            if (string.IsNullOrWhiteSpace(oe.TipoIncidencia)) throw new LogicaExcepciones("Debe indicar el tipo de incidencia.", null);
            if (oe.TipoIncidencia.Length > 100) throw new LogicaExcepciones("El tipo no puede superar los 100 caracteres.", null);
            if (string.IsNullOrWhiteSpace(oe.Descripcion)) throw new LogicaExcepciones("Debe indicar una descripción.", null);
            if (oe.Descripcion.Trim().Length < 10) throw new LogicaExcepciones("La descripción debe tener al menos 10 caracteres.", null);
            if (oe.IdArea <= 0) throw new LogicaExcepciones("Debe seleccionar un área válida.", null);
            if (oe.IdPrioridad <= 0) throw new LogicaExcepciones("Debe seleccionar una prioridad válida.", null);
            if (oe.IdTecnicoAsignado.HasValue) ValidarTecnico(oe.IdTecnicoAsignado.Value);
        }

        private void ValidarTecnico(int idUsuario)
        {
            List<sp_Usuarios_ListarResult> usuarios = UsuarioCD.ListarUsuarios();
            sp_Usuarios_ListarResult usuario = usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);

            if (usuario == null) throw new LogicaExcepciones("El técnico asignado no existe.", null);
            if (usuario.Rol != "Técnico") throw new LogicaExcepciones("El usuario asignado no tiene el rol de Técnico.", null);
            if (!usuario.Estado) throw new LogicaExcepciones("El técnico asignado está inactivo.", null);
        }

        public Incidencia BuscarPorTicket(string numeroTicket)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(numeroTicket)) throw new LogicaExcepciones("Debe indicar un número de ticket.", null);

                List<Incidencia> incidencias = ShowIncidencia();
                return incidencias.FirstOrDefault(i => string.Equals(i.NumeroTicket, numeroTicket.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al buscar incidencia por ticket", ex);
            }
        }

        public string AceptarIncidenciaPorTelegram(int idIncidencia, long chatIdTelegram)
        {
            try
            {
                Usuario tecnico = new UsuarioLN().BuscarPorChatId(chatIdTelegram);

                if (tecnico == null || tecnico.Rol != "Técnico" || !tecnico.Estado)
                {
                    return "❌ No estás registrado como técnico activo o tu cuenta no está vinculada.";
                }

                int idEnProceso = ObtenerIdEstadoPorNombre(ESTADO_EN_PROCESO);
                bool asignado = IncidenciaCD.AsignarTecnicoTelegram(idIncidencia, tecnico.IdUsuario, idEnProceso);

                if (asignado)
                {
                    new AuditoriaLN().Registrar(
                        tecnico.IdUsuario,
                        $"{tecnico.Nombre} {tecnico.Apellido}",
                        "Asignar",
                        "Incidencia",
                        idIncidencia,
                        "Aceptó el ticket desde Telegram"
                    );
                    return "SUCCESS";
                }
                else return "⚠️ Lo sentimos, esta incidencia ya fue tomada por otro técnico.";
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al procesar la aceptación de la incidencia", ex);
            }
        }

        public string CambiarEstadoPorTelegram(int idIncidencia, long chatIdTelegram, string nuevoEstadoNombre, string observacion)
        {
            try
            {
                Usuario tecnico = new UsuarioLN().BuscarPorChatId(chatIdTelegram);
                if (tecnico == null) return "❌ Usuario no encontrado o no vinculado.";

                int nuevoIdEstado = ObtenerIdEstadoPorNombre(nuevoEstadoNombre);
                bool actualizado = IncidenciaCD.ActualizarEstadoTelegram(idIncidencia, tecnico.IdUsuario, nuevoIdEstado, observacion);

                if (actualizado)
                {
                    new AuditoriaLN().Registrar(
                        tecnico.IdUsuario,
                        $"{tecnico.Nombre} {tecnico.Apellido}",
                        "Modificar",
                        "Incidencia",
                        idIncidencia,
                        $"Cambió el estado a '{nuevoEstadoNombre}' desde Telegram. Observación: {observacion}"
                    );
                    return $"SUCCESS_{nuevoEstadoNombre}";
                }
                else return "⛔ No tienes permiso para modificar esta incidencia (no está asignada a ti o no existe).";
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al cambiar el estado de la incidencia por Telegram", ex);
            }
        }

        // 1. Creamos una clase propia en la capa Lógica que el Bot sí puede ver
        public class MensajeBot
        {
            public long ChatId { get; set; }
            public int MessageId { get; set; }
        }

        public void RegistrarMensajeTelegram(int idIncidencia, long chatId, int messageId)
        {
            IncidenciaCD.RegistrarMensajeTelegram(idIncidencia, chatId, messageId);
        }

        // 2. Cambiamos el tipo de retorno a MensajeBot y transformamos la lista
        public List<MensajeBot> ObtenerMensajesTelegram(int idIncidencia)
        {
            var listaDatos = IncidenciaCD.ObtenerMensajesTelegram(idIncidencia);

            return listaDatos.Select(m => new MensajeBot
            {
                ChatId = m.ChatId,
                MessageId = m.MessageId
            }).ToList();
        }
    }
}