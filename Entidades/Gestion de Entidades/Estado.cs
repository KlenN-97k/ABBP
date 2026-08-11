using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Gestion_de_Entidades
{
    public class Estado
    {
        private int idEstado;
        private string nombre;

        public Estado()
        {
        }

        public Estado(int idEstado, string nombre)
        {
            this.IdEstado = idEstado;
            this.Nombre = nombre;
        }

        public int IdEstado { get => idEstado; set => idEstado = value; }
        public string Nombre { get => nombre; set => nombre = value; }
    }

}
