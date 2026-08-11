using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reportes
{
    public class ReportesExcepciones : Exception
    {
        public ReportesExcepciones(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
