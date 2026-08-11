using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    /// <summary>
    /// Traduce SqlException (violaciones de UNIQUE, FOREIGN KEY, etc.) a mensajes
    /// entendibles para el usuario final, en vez de mostrar el texto crudo de SQL Server.
    /// Se usa desde las clases ...CD antes de envolver el error en DatosExcepciones.
    /// </summary>
    public static class SqlErrorTraductor
    {
        // Números de error de SQL Server que nos interesa traducir.
        private const int ERROR_UNIQUE_VIOLATION_1 = 2627; // PRIMARY KEY o UNIQUE con nombre
        private const int ERROR_UNIQUE_VIOLATION_2 = 2601; // Índice UNIQUE sin restricción con nombre
        private const int ERROR_FOREIGN_KEY_VIOLATION = 547;

        public static string Traducir(SqlException ex, string mensajePorDefecto)
        {
            switch (ex.Number)
            {
                case ERROR_UNIQUE_VIOLATION_1:
                case ERROR_UNIQUE_VIOLATION_2:
                    return TraducirUnique(ex);

                case ERROR_FOREIGN_KEY_VIOLATION:
                    return TraducirForeignKey(ex);

                default:
                    return mensajePorDefecto;
            }
        }

        private static string TraducirUnique(SqlException ex)
        {
            if (Contiene(ex.Message, "UQ_Usuarios_Correo"))
                return "Ya existe un usuario registrado con ese correo.";

            if (Contiene(ex.Message, "UQ_Usuarios_Usuario"))
                return "Ya existe un usuario registrado con ese nombre de usuario.";

            return "Ya existe un registro con esos datos (violación de restricción única).";
        }

        private static string TraducirForeignKey(SqlException ex)
        {
            bool esEliminacion = Contiene(ex.Message, "DELETE statement");

            if (Contiene(ex.Message, "FK_Incidencias_Areas"))
                return esEliminacion
                    ? "No se puede eliminar esta área porque tiene incidencias asociadas."
                    : "El área seleccionada no es válida.";

            if (Contiene(ex.Message, "FK_Incidencias_Prioridades"))
                return esEliminacion
                    ? "No se puede eliminar esta prioridad porque tiene incidencias asociadas."
                    : "La prioridad seleccionada no es válida.";

            if (Contiene(ex.Message, "FK_Incidencias_Estados"))
                return esEliminacion
                    ? "No se puede eliminar este estado porque tiene incidencias asociadas."
                    : "El estado seleccionado no es válido.";

            if (Contiene(ex.Message, "FK_Incidencias_Tecnico"))
                return esEliminacion
                    ? "No se puede eliminar este usuario porque tiene incidencias asignadas como técnico."
                    : "El técnico seleccionado no es válido.";

            return esEliminacion
                ? "No se puede eliminar este registro porque está siendo utilizado por otros datos."
                : "Uno de los datos referenciados no es válido.";
        }

        private static bool Contiene(string mensaje, string texto)
        {
            return mensaje.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
