using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Gestion_de_Entidades
{
    public class Prioridad
    {
        private int idPrioridad;
        private string nombre;

        public Prioridad()
        {
        }

        public Prioridad(int idPrioridad, string nombre)
        {
            this.IdPrioridad = idPrioridad;
            this.Nombre = nombre;
        }

        public int IdPrioridad { get => idPrioridad; set => idPrioridad = value; }
        public string Nombre { get => nombre; set => nombre = value; }
    }

}
