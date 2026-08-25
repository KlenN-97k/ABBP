using System;
using System.Configuration;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups; // Requisito clave en la capa Bot

namespace Bot
{
    public static class TelegramNotificador
    {
        private static readonly Lazy<TelegramBotClient> cliente = new Lazy<TelegramBotClient>(() =>
        {
            string token = ConfigurationManager.AppSettings["TelegramBotToken"];
            return new TelegramBotClient(token);
        });

        // Modificado para aceptar el ID de la incidencia como un int opcional
        public static async Task EnviarMensajeAsync(long chatId, string mensaje, int? idIncidenciaParaAceptar = null)
        {
            try
            {
                InlineKeyboardMarkup teclado = null;

                // Si nos mandan un ID, armamos el botón flotante
                if (idIncidenciaParaAceptar.HasValue)
                {
                    teclado = new InlineKeyboardMarkup(new[]
                    {
                        new[] { InlineKeyboardButton.WithCallbackData("🙋‍♂️ Aceptar Ticket", $"aceptar_{idIncidenciaParaAceptar.Value}") }
                    });
                }

                // Sintaxis correcta para v22+
                await cliente.Value.SendMessage(chatId, mensaje, replyMarkup: teclado).ConfigureAwait(false);
            }
            catch
            {
                // Notificación "best effort" - ignora fallos de red o bloqueos
            }
        }

        public static void EnviarMensaje(long chatId, string mensaje, int? idIncidenciaParaAceptar = null)
        {
            EnviarMensajeAsync(chatId, mensaje, idIncidenciaParaAceptar).GetAwaiter().GetResult();
        }
    }
}