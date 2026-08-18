using System;
using System.Configuration;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Bot
{
    public static class TelegramNotificador
    {
        private static readonly Lazy<TelegramBotClient> cliente = new Lazy<TelegramBotClient>(() =>
        {
            string token = ConfigurationManager.AppSettings["TelegramBotToken"];
            return new TelegramBotClient(token);
        });

        public static async Task EnviarMensajeAsync(long chatId, string mensaje)
        {
            try
            {
                await cliente.Value.SendMessage(chatId, mensaje).ConfigureAwait(false);
            }
            catch
            {
                // Notificación "best effort": si falla (usuario bloqueó el bot, sin
                // internet, etc.) no debe romper el flujo de guardar la incidencia.
            }
        }

        public static void EnviarMensaje(long chatId, string mensaje)
        {
            EnviarMensajeAsync(chatId, mensaje).GetAwaiter().GetResult();
        }
    }
}