using Datos.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Gestion_de_Datos
{
    public class UsuarioCD
    {
        public static List<sp_Usuarios_ListarResult> ListarUsuarios()
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    return DB.sp_Usuarios_Listar().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al ejecutar el procedimiento Listar usuarios", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static sp_Usuarios_LoginResult BuscarPorUsuario(string usuario)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    // sp_Usuarios_Login filtra por Usuario + Estado activo en el servidor.
                    // No compara Password aquí: eso lo hace UsuarioLN con BCrypt.
                    return DB.sp_Usuarios_Login(usuario).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al ejecutar el procedimiento Login de usuario", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void InsertarUsuario(Entidades.Gestion_de_Entidades.Usuario oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Usuarios_Insertar(oc.Nombre, oc.Apellido, oc.Correo, oc.UsuarioLogin, oc.Password, oc.Rol, oc.Estado);
                    DB.SubmitChanges();
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al insertar en la tabla Usuarios"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al insertar en la tabla Usuarios", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void ModificarUsuario(Entidades.Gestion_de_Entidades.Usuario oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    System.Data.Linq.Binary fotoBinary = oc.FotoPerfil != null
                        ? new System.Data.Linq.Binary(oc.FotoPerfil)
                        : null;

                    DB.sp_Usuarios_Modificar(oc.IdUsuario, oc.Nombre, oc.Apellido, oc.Correo, oc.UsuarioLogin, oc.Password, oc.Rol, oc.Estado, oc.TelegramChatId, fotoBinary);
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al modificar en la tabla Usuarios"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al modificar en la tabla Usuarios", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void EliminarUsuario(Entidades.Gestion_de_Entidades.Usuario oc)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Usuarios_Eliminar(oc.IdUsuario);
                    DB.SubmitChanges();
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatosExcepciones(SqlErrorTraductor.Traducir(sqlEx, "Error al eliminar en la tabla Usuarios"), sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al eliminar en la tabla Usuarios", ex);
            }
            finally
            {
                DB = null;
            }
        }
        public static void RegistrarIntentoFallido(int idUsuario)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Usuarios_RegistrarIntentoFallido(idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al registrar intento fallido", ex);
            }
            finally
            {
                DB = null;
            }
        }

        public static void ResetearIntentos(int idUsuario)
        {
            BDIncidenciasDataContext DB = null;
            try
            {
                using (DB = new BDIncidenciasDataContext())
                {
                    DB.sp_Usuarios_ResetearIntentos(idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new DatosExcepciones("Error al resetear intentos de login", ex);
            }
            finally
            {
                DB = null;
            }
        }
    }
}
