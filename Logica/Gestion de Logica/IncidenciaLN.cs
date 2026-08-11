using Datos.Base_de_Datos;
using Datos.Gestion_de_Datos;
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
        /*Nombres oficiales del catalogo Estados. 
        Se resuelvem em tiempo de ejecucion para no depender el orden de los INSERT iniciales.
         ni que el IDENTITY de Estados coincida siempre con 1/2/3*/

        private const string ESTADO_PENDIENTE = "Pendiente";
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

                    // Campos descriptivos que trae el JOIN del SP, para que Presentación
                    // no tenga que resolver IDs a nombres por su cuenta.
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
            finally
            {
            }
            return lista;
        }

        public bool InsertIncidencia(Incidencia oe)
        {
            try
            {
                ValidarIncidencia(oe);

                // Regla de Negocio: Toda incidencia nueva nace en estado 'Pendiente'
                oe.IdEstado = ObtenerIdEstadoPorNombre(ESTADO_PENDIENTE);

                IncidenciaCD.InsertarIncidencia(oe);
                return true;
            }
            catch (LogicaExcepciones)
            {
                // Ya trae un mensaje claro (validación o estado no encontrado); no lo reenvolvemos
                // para no perderlo detrás de un mensaje genérico.
                throw;
            }
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

                // Regla de Negocio: no se puede resolver/cerrar una incidencia sin técnico asignado.
                if (esResueltaOCerrada && oe.IdTecnicoAsignado == null)
                {
                    throw new LogicaExcepciones(
                        "No se puede marcar la incidencia como Resuelto/Cerrado sin un técnico asignado.",
                        null);
                }

                // Regla de Negocio: Si el estado es Resuelto o Cerrado, asignar fecha de solución
                if (esResueltaOCerrada && oe.FechaSolucion == null)
                {
                    oe.FechaSolucion = DateTime.Now;
                }

                IncidenciaCD.ModificarIncidencia(oe);
                return true;
            }
            catch (LogicaExcepciones)
            {
                throw;
            }
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

        /// <summary>
        /// Consulta el catálogo dbo.Estados y resuelve el IdEstado correspondiente a un nombre.
        /// </summary>
        private int ObtenerIdEstadoPorNombre(string nombreEstado)
        {
            List<sp_Estados_ListarResult> estados = EstadoCD.ListarEstados();
            return ObtenerIdEstadoPorNombre(estados, nombreEstado);
        }

        /// <summary>
        /// Resuelve el IdEstado correspondiente a un nombre a partir de una lista ya obtenida
        /// del catálogo (evita consultas repetidas a la BD cuando se necesita más de un estado).
        /// </summary>
        private int ObtenerIdEstadoPorNombre(List<sp_Estados_ListarResult> estados, string nombreEstado)
        {
            sp_Estados_ListarResult estado = estados.FirstOrDefault(e => e.Nombre == nombreEstado);

            if (estado == null)
            {
                throw new LogicaExcepciones(
                    $"No se encontró el estado '{nombreEstado}' en el catálogo dbo.Estados. Verifique los datos iniciales de la tabla Estados.",
                    null);
            }

            return estado.IdEstado;
        }

        /// <summary>
        /// Valida los datos mínimos de una incidencia antes de mandarla a la BD.
        /// Evita depender de que SQL Server tire un error crudo (ej. truncamiento de
        /// VARCHAR, o violación de FK) para detectar algo que se pudo prevenir acá.
        /// </summary>
        private void ValidarIncidencia(Incidencia oe)
        {
            if (string.IsNullOrWhiteSpace(oe.Empleado))
            {
                throw new LogicaExcepciones("Debe indicar el nombre del empleado que reporta la incidencia.", null);
            }
            if (oe.Empleado.Length > 150)
            {
                throw new LogicaExcepciones("El nombre del empleado no puede superar los 150 caracteres.", null);
            }

            if (string.IsNullOrWhiteSpace(oe.TipoIncidencia))
            {
                throw new LogicaExcepciones("Debe indicar el tipo de incidencia.", null);
            }
            if (oe.TipoIncidencia.Length > 100)
            {
                throw new LogicaExcepciones("El tipo de incidencia no puede superar los 100 caracteres.", null);
            }

            if (string.IsNullOrWhiteSpace(oe.Descripcion))
            {
                throw new LogicaExcepciones("Debe indicar una descripción de la incidencia.", null);
            }
            if (oe.Descripcion.Trim().Length < 10)
            {
                throw new LogicaExcepciones("La descripción debe tener al menos 10 caracteres.", null);
            }

            if (oe.IdArea <= 0)
            {
                throw new LogicaExcepciones("Debe seleccionar un área válida.", null);
            }

            if (oe.IdPrioridad <= 0)
            {
                throw new LogicaExcepciones("Debe seleccionar una prioridad válida.", null);
            }

            // La FK solo garantiza que IdTecnicoAsignado exista como Usuario; no que sea
            // un Técnico activo. Esa regla de negocio se valida acá.
            if (oe.IdTecnicoAsignado.HasValue)
            {
                ValidarTecnico(oe.IdTecnicoAsignado.Value);
            }
        }

        /// <summary>
        /// Verifica que el usuario asignado como técnico exista, tenga Rol = 'Técnico'
        /// y esté activo (Estado = true).
        /// </summary>
        private void ValidarTecnico(int idUsuario)
        {
            List<sp_Usuarios_ListarResult> usuarios = UsuarioCD.ListarUsuarios();
            sp_Usuarios_ListarResult usuario = usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);

            if (usuario == null)
            {
                throw new LogicaExcepciones("El técnico asignado no existe.", null);
            }

            if (usuario.Rol != "Técnico")
            {
                throw new LogicaExcepciones("El usuario asignado como técnico no tiene el rol de Técnico.", null);
            }

            if (!usuario.Estado)
            {
                throw new LogicaExcepciones("El técnico asignado está inactivo.", null);
            }
        }


        public Incidencia BuscarPorTicket(string numeroTicket)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(numeroTicket))
                    throw new LogicaExcepciones("Debe indicar un número de ticket.", null);

                List<Incidencia> incidencias = ShowIncidencia();
                return incidencias.FirstOrDefault(i =>
                    string.Equals(i.NumeroTicket, numeroTicket.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            catch (LogicaExcepciones)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al buscar incidencia por ticket", ex);
            }
        }
    }

}
