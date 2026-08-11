using Datos.Base_de_Datos;
using Datos.Gestion_de_Datos;
using Entidades.Gestion_de_Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estado = Entidades.Gestion_de_Entidades.Estado;

namespace Logica.Gestion_de_Logica
{
    public class EstadoLN
    {
        public List<Estado> ShowEstado()
        {
            List<Estado> lista = new List<Estado>();
            try
            {
                List<sp_Estados_ListarResult> auxLista = EstadoCD.ListarEstados();
                foreach (sp_Estados_ListarResult obj in auxLista)
                    lista.Add(new Estado(obj.IdEstado, obj.Nombre));
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al mostrar estados", ex);
            }
            return lista;
        }

        public bool InsertEstado(Estado oe)
        {
            try
            {
                ValidarEstado(oe);
                EstadoCD.InsertarEstado(oe);
                return true;
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al insertar estado en la BD", ex);
            }
        }

        public bool UpdateEstado(Estado oe)
        {
            try
            {
                ValidarEstado(oe);
                EstadoCD.ModificarEstado(oe);
                return true;
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al actualizar estado en la BD", ex);
            }
        }

        public bool DeleteEstado(Estado oe)
        {
            try
            {
                EstadoCD.EliminarEstado(oe);
                return true;
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al eliminar estado en la BD", ex);
            }
        }

        private void ValidarEstado(Estado oe)
        {
            if (string.IsNullOrWhiteSpace(oe.Nombre))
                throw new LogicaExcepciones("Debe indicar el nombre del estado.", null);
            if (oe.Nombre.Length > 50)
                throw new LogicaExcepciones("El nombre del estado no puede superar los 50 caracteres.", null);

            bool duplicado = ShowEstado().Any(es =>
                es.IdEstado != oe.IdEstado &&
                string.Equals(es.Nombre.Trim(), oe.Nombre.Trim(), StringComparison.OrdinalIgnoreCase));
            if (duplicado)
                throw new LogicaExcepciones("Ya existe un estado con ese nombre.", null);
        }
    }

}
