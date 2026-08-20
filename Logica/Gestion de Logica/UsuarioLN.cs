using Datos;
using Datos.Base_de_Datos;
using Datos.Gestion_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Usuario = Entidades.Gestion_de_Entidades.Usuario;

namespace Logica.Gestion_de_Logica
{
    public class UsuarioLN
    {
        private static readonly string[] ROLES_VALIDOS = { "Administrador", "Técnico", "Usuario" };
        private static readonly Regex REGEX_CORREO = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public List<Usuario> ShowUsuario()
        {
            List<Usuario> lista = new List<Usuario>();
            Usuario oc;
            try
            {
                List<sp_Usuarios_ListarResult> auxLista = UsuarioCD.ListarUsuarios();
                foreach (sp_Usuarios_ListarResult obj in auxLista)
                {
                    byte[] fotoBytes = obj.FotoPerfil != null ? obj.FotoPerfil.ToArray() : null;
                    oc = new Usuario(obj.IdUsuario, obj.Nombre, obj.Apellido, obj.Correo, obj.Usuario, obj.Password, obj.Rol, obj.Estado, obj.TelegramChatId, fotoBytes);
                    lista.Add(oc);
                }
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al mostrar usuarios", ex);
            }
            return lista;
        }

        public Usuario Login(string usuarioLogin, string password)
        {
            try
            {
                sp_Usuarios_LoginResult encontrado = UsuarioCD.BuscarPorUsuario(usuarioLogin);

                if (encontrado == null)
                {
                    return null; // no existe o no está activo
                }

                if (encontrado.BloqueadoHasta.HasValue && encontrado.BloqueadoHasta.Value > DateTime.Now)
                {
                    int minutosRestantes = (int)Math.Ceiling((encontrado.BloqueadoHasta.Value - DateTime.Now).TotalMinutes);
                    throw new LogicaExcepciones(
                        $"Cuenta bloqueada temporalmente por intentos fallidos. Intenta de nuevo en {minutosRestantes} minuto(s).",
                        null);
                }

                bool passwordValido = BCrypt.Net.BCrypt.Verify(password, encontrado.Password);

                if (!passwordValido)
                {
                    UsuarioCD.RegistrarIntentoFallido(encontrado.IdUsuario);
                    return null;
                }

                UsuarioCD.ResetearIntentos(encontrado.IdUsuario);

                byte[] fotoBytes = encontrado.FotoPerfil != null ? encontrado.FotoPerfil.ToArray() : null;

                return new Usuario(
                    encontrado.IdUsuario, encontrado.Nombre, encontrado.Apellido, encontrado.Correo,
                    encontrado.Usuario, encontrado.Password, encontrado.Rol, encontrado.Estado,
                    encontrado.TelegramChatId, fotoBytes
                );
            }
            catch (LogicaExcepciones)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al validar credenciales de usuario", ex);
            }
        }

        private const int TAMANO_MAXIMO_FOTO_BYTES = 2 * 1024 * 1024; // 2 MB

        public bool ActualizarFotoPerfil(Usuario oe, byte[] fotoBytes)
        {
            try
            {
                if (fotoBytes != null && fotoBytes.Length > TAMANO_MAXIMO_FOTO_BYTES)
                {
                    throw new LogicaExcepciones("La imagen no puede superar los 2 MB.", null);
                }

                oe.FotoPerfil = fotoBytes;
                return UpdateUsuario(oe);
            }
            catch (LogicaExcepciones) { throw; }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al actualizar la foto de perfil", ex);
            }
        }

        public bool InsertUsuario(Usuario oe)
        {
            try
            {
                ValidarUsuario(oe);

                if (string.IsNullOrWhiteSpace(oe.Password))
                    throw new LogicaExcepciones("Debe indicar una contraseña.", null);

                if (oe.Password.Length < 6)
                    throw new LogicaExcepciones("La contraseña debe tener al menos 6 caracteres.", null);

                oe.Password = BCrypt.Net.BCrypt.HashPassword(oe.Password);
                UsuarioCD.InsertarUsuario(oe);
                return true;
            }
            catch (LogicaExcepciones) { throw; }
            catch (DatosExcepciones dex) { throw new LogicaExcepciones(dex.Message, dex); }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al insertar usuario en la BD", ex);
            }
        }

        public bool UpdateUsuario(Usuario oe)
        {
            try
            {
                ValidarUsuario(oe);
                UsuarioCD.ModificarUsuario(oe);
                return true;
            }
            catch (LogicaExcepciones) { throw; }
            catch (DatosExcepciones dex) { throw new LogicaExcepciones(dex.Message, dex); }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al actualizar usuario en la BD", ex);
            }
        }

        public bool CambiarPassword(Usuario oe, string nuevaPasswordPlano)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nuevaPasswordPlano) || nuevaPasswordPlano.Length < 6)
                    throw new LogicaExcepciones("La contraseña debe tener al menos 6 caracteres.", null);

                oe.Password = BCrypt.Net.BCrypt.HashPassword(nuevaPasswordPlano);
                UsuarioCD.ModificarUsuario(oe);
                return true;
            }
            catch (LogicaExcepciones) { throw; }
            catch (DatosExcepciones dex) { throw new LogicaExcepciones(dex.Message, dex); }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al cambiar la contraseña del usuario", ex);
            }
        }

        public bool DeleteUsuario(Usuario oe)
        {
            try
            {
                UsuarioCD.EliminarUsuario(oe);
                return true;
            }
            catch (DatosExcepciones dex) { throw new LogicaExcepciones(dex.Message, dex); }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al eliminar usuario en la BD", ex);
            }
        }

        private void ValidarUsuario(Usuario oe)
        {
            if (string.IsNullOrWhiteSpace(oe.Nombre))
                throw new LogicaExcepciones("Debe indicar el nombre del usuario.", null);
            if (oe.Nombre.Length > 100)
                throw new LogicaExcepciones("El nombre no puede superar los 100 caracteres.", null);

            if (string.IsNullOrWhiteSpace(oe.Apellido))
                throw new LogicaExcepciones("Debe indicar el apellido del usuario.", null);
            if (oe.Apellido.Length > 100)
                throw new LogicaExcepciones("El apellido no puede superar los 100 caracteres.", null);

            if (string.IsNullOrWhiteSpace(oe.Correo))
                throw new LogicaExcepciones("Debe indicar un correo.", null);
            if (oe.Correo.Length > 150)
                throw new LogicaExcepciones("El correo no puede superar los 150 caracteres.", null);
            if (!REGEX_CORREO.IsMatch(oe.Correo))
                throw new LogicaExcepciones("El correo no tiene un formato válido.", null);

            if (string.IsNullOrWhiteSpace(oe.UsuarioLogin))
                throw new LogicaExcepciones("Debe indicar un nombre de usuario.", null);
            if (oe.UsuarioLogin.Length > 50)
                throw new LogicaExcepciones("El nombre de usuario no puede superar los 50 caracteres.", null);

            if (!ROLES_VALIDOS.Contains(oe.Rol))
                throw new LogicaExcepciones("El rol debe ser Administrador, Técnico o Usuario.", null);
        }

        public string GenerarPasswordTemporal(Usuario oe)
        {
            try
            {
                string passwordTemporal = CrearPasswordAleatoria(10);
                CambiarPassword(oe, passwordTemporal);
                return passwordTemporal;
            }
            catch (LogicaExcepciones)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al generar contraseña temporal", ex);
            }
        }

        private static string CrearPasswordAleatoria(int longitud)
        {
            // Sin 0/O ni 1/l/I: evita confusiones al transcribirla a mano o leerla por teléfono.
            const string caracteres = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";

            byte[] bytes = new byte[longitud];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            var resultado = new StringBuilder(longitud);
            foreach (byte b in bytes)
            {
                resultado.Append(caracteres[b % caracteres.Length]);
            }
            return resultado.ToString();
        }

        /// <summary>
        /// Vincula un chat de Telegram a una cuenta existente. Valida usuario/password
        /// igual que Login(); si son correctos, guarda el ChatId en esa cuenta.
        /// </summary>
        public bool RegistrarChatTelegram(string usuarioLogin, string password, long chatId)
        {
            Usuario usuario = Login(usuarioLogin, password);
            if (usuario == null)
            {
                throw new LogicaExcepciones("Usuario o contraseña incorrectos.", null);
            }

            // NUEVO: Buscar si otra cuenta ya tiene este ChatId y desvincularla
            List<Usuario> todosLosUsuarios = ShowUsuario();
            foreach (var u in todosLosUsuarios.Where(x => x.TelegramChatId == chatId))
            {
                u.TelegramChatId = null;
                UpdateUsuario(u); // Guardamos la cuenta vieja sin el Telegram vinculado
            }

            // Vincular al nuevo usuario
            usuario.TelegramChatId = chatId;
            return UpdateUsuario(usuario);
        }

        /// <summary>
        /// Busca qué usuario del sistema corresponde a un chat de Telegram ya vinculado.
        /// Devuelve null si ese chat no está registrado a ninguna cuenta.
        /// </summary>
        public Usuario BuscarPorChatId(long chatId)
        {
            try
            {
                List<Usuario> usuarios = ShowUsuario();
                return usuarios.FirstOrDefault(u => u.TelegramChatId == chatId);
            }
            catch (Exception ex)
            {
                throw new LogicaExcepciones("Error al buscar usuario por chat de Telegram", ex);
            }
        }
    }
}