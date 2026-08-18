using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Presentacion
{
    public static class CorreoService
    {
        public static void EnviarCorreoConAdjunto(string destinatario, string asunto, string cuerpo, byte[] adjunto, string nombreAdjunto)
        {
            string servidor = ConfigurationManager.AppSettings["SmtpServidor"];
            int puerto = int.Parse(ConfigurationManager.AppSettings["SmtpPuerto"]);
            string usuario = ConfigurationManager.AppSettings["SmtpUsuario"];
            string password = ConfigurationManager.AppSettings["SmtpPassword"];
            bool usarSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpUsarSsl"] ?? "true");

            using (MailMessage mensaje = new MailMessage())
            using (var ms = new MemoryStream(adjunto))
            {
                mensaje.From = new MailAddress(usuario, "Sistema de Incidencias APPB");
                mensaje.To.Add(destinatario);
                mensaje.Subject = asunto;
                mensaje.Body = cuerpo;
                mensaje.Attachments.Add(new Attachment(ms, nombreAdjunto));

                using (SmtpClient cliente = new SmtpClient(servidor, puerto))
                {
                    cliente.EnableSsl = usarSsl;
                    cliente.Credentials = new NetworkCredential(usuario, password);
                    cliente.Send(mensaje);
                }
            }
        }
    }
}