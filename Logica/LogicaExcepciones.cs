using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaExcepciones : Exception
    {
        public LogicaExcepciones(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
