using MySqlConnector;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public static class ConexionMySQL
    {
        public static MySqlConnection ObtenerConexion()
        {
            string cadena = ConfigurationManager.ConnectionStrings["DBIncidenciasConnectionString"].ConnectionString;
            return new MySqlConnection(cadena);
        }
    }
}
