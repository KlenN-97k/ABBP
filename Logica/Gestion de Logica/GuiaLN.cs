using Datos.Base_de_Datos;
using Datos.Gestion_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guia = Entidades.Gestion_de_Entidades.Guia;

namespace Logica.Gestion_de_Logica
{
    public class GuiaLN
    {
        public List<Guia> ShowGuia()
        {
            List<Guia> lista = new List<Guia>();
            try
            {
                List<sp_Guias_ListarResult> auxLista = GuiaCD.ListarGuias();
                foreach (sp_Guias_ListarResult obj in auxLista)
                    lista.Add(new Guia(obj.IdGuia, obj.Titulo, obj.Problema, obj.Solucion));
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al mostrar guías", ex);
            }
            return lista;
        }

        public bool InsertGuia(Guia oe)
        {
            try
            {
                ValidarGuia(oe);
                GuiaCD.InsertarGuia(oe);
                return true;
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al insertar guía en la BD", ex);
            }
        }

        public bool UpdateGuia(Guia oe)
        {
            try
            {
                ValidarGuia(oe);
                GuiaCD.ModificarGuia(oe);
                return true;
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al actualizar guía en la BD", ex);
            }
        }

        public bool DeleteGuia(Guia oe)
        {
            try
            {
                GuiaCD.EliminarGuia(oe);
                return true;
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al eliminar guía en la BD", ex);
            }
        }

        private void ValidarGuia(Guia oe)
        {
            if (string.IsNullOrWhiteSpace(oe.Titulo))
                throw new LogicaExcepciones("Debe indicar el título de la guía.", null);
            if (oe.Titulo.Length > 150)
                throw new LogicaExcepciones("El título no puede superar los 150 caracteres.", null);

            if (string.IsNullOrWhiteSpace(oe.Problema))
                throw new LogicaExcepciones("Debe indicar el problema que resuelve la guía.", null);

            if (string.IsNullOrWhiteSpace(oe.Solucion))
                throw new LogicaExcepciones("Debe indicar la solución de la guía.", null);

            bool duplicado = ShowGuia().Any(g =>
                g.IdGuia != oe.IdGuia &&
                string.Equals(g.Titulo.Trim(), oe.Titulo.Trim(), StringComparison.OrdinalIgnoreCase));
            if (duplicado)
                throw new LogicaExcepciones("Ya existe una guía con ese título.", null);
        }
    }

}
