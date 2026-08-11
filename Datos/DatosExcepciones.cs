using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DatosExcepciones : Exception
    {
        public DatosExcepciones(string message, Exception innerException)
            : base(message, innerException) { }
    }

}
