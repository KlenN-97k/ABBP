using Datos.Base_de_Datos;
using Datos.Gestion_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Area = Entidades.Gestion_de_Entidades.Area;

namespace Logica.Gestion_de_Logica
{
    public class AreaLN
    {
        public List<Area> ShowArea()
        {
            List<Area> lista = new List<Area>();
            try
            {
                List<sp_Areas_ListarResult> auxLista = AreaCD.ListarAreas();
                foreach (sp_Areas_ListarResult obj in auxLista)
                    lista.Add(new Area(obj.IdArea, obj.NombreArea));
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al mostrar áreas con el procedimiento", ex);
            }
            return lista;
        }

        public bool InsertArea(Area oe)
        {
            try
            {
                ValidarArea(oe);
                AreaCD.InsertarArea(oe);
                return true;
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al insertar área en la BD", ex);
            }
        }

        public bool UpdateArea(Area oe)
        {
            try
            {
                ValidarArea(oe);
                AreaCD.ModificarArea(oe);
                return true;
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al actualizar área en la BD", ex);
            }
        }

        public bool DeleteArea(Area oe)
        {
            try
            {
                AreaCD.EliminarArea(oe);
                return true;
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al eliminar área en la BD", ex);
            }
        }

        private void ValidarArea(Area oe)
        {
            if (string.IsNullOrWhiteSpace(oe.NombreArea))
                throw new LogicaExcepciones("Debe indicar el nombre del área.", null);
            if (oe.NombreArea.Length > 100)
                throw new LogicaExcepciones("El nombre del área no puede superar los 100 caracteres.", null);

            bool duplicado = ShowArea().Any(a =>
                a.IdArea != oe.IdArea &&
                string.Equals(a.NombreArea.Trim(), oe.NombreArea.Trim(), StringComparison.OrdinalIgnoreCase));
            if (duplicado)
                throw new LogicaExcepciones("Ya existe un área con ese nombre.", null);
        }
    }

}
