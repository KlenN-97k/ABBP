using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Gestion_de_Entidades
{
    public class Area
    {
        private int idArea;
        private string nombreArea;

        public Area()
        {
        }

        public Area(int idArea, string nombreArea)
        {
            this.IdArea = idArea;
            this.NombreArea = nombreArea;
        }

        public int IdArea { get => idArea; set => idArea = value; }
        public string NombreArea { get => nombreArea; set => nombreArea = value; }

    }
}
