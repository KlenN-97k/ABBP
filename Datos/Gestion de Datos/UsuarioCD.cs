using Dapper;
using Entidades.Gestion_de_Entidades;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos.Gestion_de_Datos
{
    public class UsuarioCD
    {
        public static List<Usuario> ListarUsuarios()
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = @"
                        SELECT IdUsuario, Nombre, Apellido, Correo,
                               Usuario AS UsuarioLogin, Password, Rol, Estado,
                               TelegramChatId, FotoPerfil
                        FROM Usuarios
                        ORDER BY Apellido, Nombre;";

                    return conexion.Query<Usuario>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al listar usuarios", ex);
            }
        }

        // Reemplaza a sp_Usuarios_LoginResult: incluye IntentosFallidos/BloqueadoHasta,
        // que no viven en la entidad Usuario (solo se usan dentro del proceso de Login).
        public class LoginResultDTO
        {
            public int IdUsuario { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Correo { get; set; }
            public string UsuarioLogin { get; set; }
            public string Password { get; set; }
            public string Rol { get; set; }
            public bool Estado { get; set; }
            public long? TelegramChatId { get; set; }
            public int IntentosFallidos { get; set; }
            public DateTime? BloqueadoHasta { get; set; }
            public byte[] FotoPerfil { get; set; }
        }

        public static LoginResultDTO BuscarPorUsuario(string usuario)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = @"
                        SELECT IdUsuario, Nombre, Apellido, Correo,
                               Usuario AS UsuarioLogin, Password, Rol, Estado,
                               TelegramChatId, IntentosFallidos, BloqueadoHasta, FotoPerfil
                        FROM Usuarios
                        WHERE Usuario = @usuario AND Estado = 1;";

                    return conexion.QueryFirstOrDefault<LoginResultDTO>(sql, new { usuario });
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al buscar el usuario para login", ex);
            }
        }

        public static void InsertarUsuario(Usuario oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = @"
                        INSERT INTO Usuarios (Nombre, Apellido, Correo, Usuario, Password, Rol, Estado)
                        VALUES (@Nombre, @Apellido, @Correo, @UsuarioLogin, @Password, @Rol, @Estado);";

                    conexion.Execute(sql, oc);
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Usuarios: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Usuarios", ex);
            }
        }

        public static void ModificarUsuario(Usuario oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = @"
                        UPDATE Usuarios
                        SET Nombre = @Nombre,
                            Apellido = @Apellido,
                            Correo = @Correo,
                            Usuario = @UsuarioLogin,
                            Password = @Password,
                            Rol = @Rol,
                            Estado = @Estado,
                            TelegramChatId = @TelegramChatId,
                            FotoPerfil = @FotoPerfil
                        WHERE IdUsuario = @IdUsuario;";

                    conexion.Execute(sql, oc);
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Usuarios: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Usuarios", ex);
            }
        }

        public static void EliminarUsuario(Usuario oc)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute("DELETE FROM Usuarios WHERE IdUsuario = @IdUsuario;", new { oc.IdUsuario });
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Usuarios: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Usuarios", ex);
            }
        }

        public static void RegistrarIntentoFallido(int idUsuario)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    string sql = @"
                        UPDATE Usuarios
                        SET IntentosFallidos = IntentosFallidos + 1,
                            BloqueadoHasta = CASE
                                WHEN IntentosFallidos + 1 >= 5 THEN DATE_ADD(NOW(), INTERVAL 5 MINUTE)
                                ELSE BloqueadoHasta
                            END
                        WHERE IdUsuario = @idUsuario;";

                    conexion.Execute(sql, new { idUsuario });
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al registrar intento fallido", ex);
            }
        }

        public static void ResetearIntentos(int idUsuario)
        {
            try
            {
                using (var conexion = ConexionMySQL.ObtenerConexion())
                {
                    conexion.Execute(
                        "UPDATE Usuarios SET IntentosFallidos = 0, BloqueadoHasta = NULL WHERE IdUsuario = @idUsuario;",
                        new { idUsuario });
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al resetear intentos de login", ex);
            }
        }
    }
}