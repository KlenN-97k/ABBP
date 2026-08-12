using Logica;
using Logica.Gestion_de_Logica;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot
{
    class Program
    {
        static TelegramBotClient botClient;

        static async Task Main(string[] args)
        {
            string token = ConfigurationManager.AppSettings["TelegramBotToken"];

            using (var cts = new CancellationTokenSource())
            {
                botClient = new TelegramBotClient(token, cancellationToken: cts.Token);

                var me = await botClient.GetMe();
                botClient.OnMessage += OnMessage;

                Console.WriteLine($"Bot iniciado: @{me.Username}");
                Console.WriteLine("Presiona ENTER para detener...");
                Console.ReadLine();

                cts.Cancel();
            }
        }

        static async Task OnMessage(Message msg, UpdateType type)
        {
            if (msg.Text == null) return;

            string[] partes = msg.Text.Trim().Split(' ');
            string comando = partes[0].ToLower();

            switch (comando)
            {
                case "/registrar":
                    await ManejarRegistrar(msg, partes);
                    break;

                default:
                    await botClient.SendMessage(msg.Chat, "Comando no reconocido. Usa /registrar usuario contraseña");
                    break;
            }
        }

        static async Task ManejarRegistrar(Message msg, string[] partes)
        {
            if (partes.Length != 3)
            {
                await botClient.SendMessage(msg.Chat, "Uso: /registrar usuario contraseña");
                return;
            }

            string usuario = partes[1];
            string password = partes[2];

            try
            {
                var usuarioLN = new UsuarioLN();
                bool ok = usuarioLN.RegistrarChatTelegram(usuario, password, msg.Chat.Id);

                if (ok)
                {
                    await botClient.SendMessage(msg.Chat, "Cuenta vinculada correctamente. Ya puedes usar /reportar y /estado.");
                }
            }
            catch (LogicaExcepciones ex)
            {
                Console.WriteLine(ex.ToString());
                await botClient.SendMessage(msg.Chat, $"No se pudo vincular: {ex.Message}");
            }
            catch (Exception)
            {
                await botClient.SendMessage(msg.Chat, "Ocurrió un error inesperado. Intenta más tarde.");
            }
        }
    }
}