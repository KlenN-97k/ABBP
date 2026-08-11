using Datos.Base_de_Datos;
using Datos.Gestion_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prioridad = Entidades.Gestion_de_Entidades.Prioridad;

namespace Logica.Gestion_de_Logica
{
    public class PrioridadLN
    {
        public List<Prioridad> ShowPrioridad()
        {
            List<Prioridad> lista = new List<Prioridad>();
            try
            {
                List<sp_Prioridades_ListarResult> auxLista = PrioridadCD.ListarPrioridades();
                foreach (sp_Prioridades_ListarResult obj in auxLista)
                    lista.Add(new Prioridad(obj.IdPrioridad, obj.Nombre));
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al mostrar prioridades", ex);
            }
            return lista;
        }

        public bool InsertPrioridad(Prioridad oe)
        {
            try
            {
                ValidarPrioridad(oe);
                PrioridadCD.InsertarPrioridad(oe);
                return true;
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al insertar prioridad en la BD", ex);
            }
        }

        public bool UpdatePrioridad(Prioridad oe)
        {
            try
            {
                ValidarPrioridad(oe);
                PrioridadCD.ModificarPrioridad(oe);
                return true;
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al actualizar prioridad en la BD", ex);
            }
        }

        public bool DeletePrioridad(Prioridad oe)
        {
            try
            {
                PrioridadCD.EliminarPrioridad(oe);
                return true;
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al eliminar prioridad en la BD", ex);
            }
        }

        private void ValidarPrioridad(Prioridad oe)
        {
            if (string.IsNullOrWhiteSpace(oe.Nombre))
                throw new LogicaExcepciones("Debe indicar el nombre de la prioridad.", null);
            if (oe.Nombre.Length > 50)
                throw new LogicaExcepciones("El nombre de la prioridad no puede superar los 50 caracteres.", null);

            bool duplicado = ShowPrioridad().Any(p =>
                p.IdPrioridad != oe.IdPrioridad &&
                string.Equals(p.Nombre.Trim(), oe.Nombre.Trim(), StringComparison.OrdinalIgnoreCase));
            if (duplicado)
                throw new LogicaExcepciones("Ya existe una prioridad con ese nombre.", null);
        }
    }

}
