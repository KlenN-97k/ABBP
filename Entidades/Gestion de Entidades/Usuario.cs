using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Gestion_de_Entidades
{
    public class Usuario
    {
        private int idUsuario;
        private string nombre;
        private string apellido;
        private string correo;
        private string usuarioLogin; // Se renombra a UsuarioLogin porque en C# una propiedad no puede llamarse igual que la clase
        private string password;
        private string rol;
        private bool estado;

        public Usuario()
        {
        }

        public Usuario(int idUsuario, string nombre, string apellido, string correo, string usuarioLogin, string password, string rol, bool estado)
        {
            this.IdUsuario = idUsuario;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Correo = correo;
            this.UsuarioLogin = usuarioLogin;
            this.Password = password;
            this.Rol = rol;
            this.Estado = estado;
        }

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string Correo { get => correo; set => correo = value; }
        public string UsuarioLogin { get => usuarioLogin; set => usuarioLogin = value; }
        public string Password { get => password; set => password = value; }
        public string Rol { get => rol; set => rol = value; }
        public bool Estado { get => estado; set => estado = value; }
    }

}
