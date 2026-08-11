using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Gestion_de_Entidades
{
    public class Guia
    {
        private int idGuia;
        private string titulo;
        private string problema;
        private string solucion;

        public Guia()
        {
        }

        public Guia(int idGuia, string titulo, string problema, string solucion)
        {
            this.IdGuia = idGuia;
            this.Titulo = titulo;
            this.Problema = problema;
            this.Solucion = solucion;
        }

        public int IdGuia { get => idGuia; set => idGuia = value; }
        public string Titulo { get => titulo; set => titulo = value; }
        public string Problema { get => problema; set => problema = value; }
        public string Solucion { get => solucion; set => solucion = value; }
    }

}
