using System;
using System.Configuration;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums; // Necesario para el formato Markdown

namespace Bot
{
    public static class TelegramNotificador
    {
        private static readonly Lazy<TelegramBotClient> cliente = new Lazy<TelegramBotClient>(() =>
        {
            string token = ConfigurationManager.AppSettings["TelegramBotToken"];
            return new TelegramBotClient(token);
        });

        // Ahora devuelve int? (El MessageId)
        public static async Task<int?> EnviarMensajeAsync(long chatId, string mensaje, int? idIncidenciaParaAceptar = null)
        {
            try
            {
                InlineKeyboardMarkup teclado = null;

                if (idIncidenciaParaAceptar.HasValue)
                {
                    teclado = new InlineKeyboardMarkup(new[]
                    {
                        new[] { InlineKeyboardButton.WithCallbackData("🙋‍♂️ Aceptar Ticket", $"aceptar_{idIncidenciaParaAceptar.Value}") }
                    });
                }

                // Aquí encendemos el ParseMode.Markdown para que formatee los asteriscos
                var msg = await cliente.Value.SendMessage(
                    chatId: chatId,
                    text: mensaje,
                    parseMode: ParseMode.Markdown,
                    replyMarkup: teclado
                ).ConfigureAwait(false);

                return msg.MessageId; // Devolvemos el ID del mensaje
            }
            catch
            {
                return null;
            }
        }

        public static int? EnviarMensaje(long chatId, string mensaje, int? idIncidenciaParaAceptar = null)
        {
            return EnviarMensajeAsync(chatId, mensaje, idIncidenciaParaAceptar).GetAwaiter().GetResult();
        }
    }
}