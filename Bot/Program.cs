using Entidades.Gestion_de_Entidades;
using Logica;
using Logica.Gestion_de_Logica;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
        private static readonly Dictionary<long, EstadoReportar> conversaciones = new Dictionary<long, EstadoReportar>();

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

            long chatId = msg.Chat.Id;
            string texto = msg.Text.Trim();

            Console.WriteLine($"[{chatId}] {texto}");

            // Si hay una conversación de /reportar en curso, el texto se procesa
            // como respuesta a esa conversación, sin importar qué haya escrito.
            if (conversaciones.ContainsKey(chatId))
            {
                if (texto.ToLower() == "/cancelar")
                {
                    conversaciones.Remove(chatId);
                    await botClient.SendMessage(chatId, "❌ Reporte cancelado.");
                    return;
                }

                await ContinuarReportar(chatId, texto);
                return;
            }

            string[] partes = texto.Split(' ');
            string comando = partes[0].ToLower();

            switch (comando)
            {
                case "/registrar":
                    await ManejarRegistrar(msg, partes);
                    break;

                case "/reportar":
                    await IniciarReportar(chatId);
                    break;

                case "/estado":
                    await ManejarEstado(chatId, partes);
                    break;

                default:
                    await botClient.SendMessage(chatId, "Comando no reconocido. Usa /registrar, /reportar o /estado.");
                    break;
            }
        }

        // ---------- /registrar ----------

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
                await botClient.SendMessage(msg.Chat, $"No se pudo vincular: {ex.Message}");
            }
            catch (Exception)
            {
                await botClient.SendMessage(msg.Chat, "Ocurrió un error inesperado. Intenta más tarde.");
            }
        }

        // ---------- /reportar ----------

        static async Task IniciarReportar(long chatId)
        {
            try
            {
                Usuario usuario = new UsuarioLN().BuscarPorChatId(chatId);

                if (usuario == null)
                {
                    await botClient.SendMessage(chatId,
                        "Primero debes vincular tu cuenta con /registrar usuario contraseña.");
                    return;
                }

                var areas = new AreaLN().ShowArea();
                string listaAreas = string.Join("\n", areas.Select(a => $"{a.IdArea}) {a.NombreArea}"));

                conversaciones[chatId] = new EstadoReportar { Paso = 1 };

                await botClient.SendMessage(chatId,
                    $"📝 Vamos a reportar una incidencia.\n\n¿En qué área ocurrió? Responde con el número:\n\n{listaAreas}\n\n(Escribe /cancelar en cualquier momento para salir)");
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"Error al iniciar el reporte: {ex.Message}");
            }
        }

        static async Task ContinuarReportar(long chatId, string texto)
        {
            EstadoReportar estado = conversaciones[chatId];

            try
            {
                switch (estado.Paso)
                {
                    case 1: // esperando número de área
                        if (!int.TryParse(texto, out int idArea) || new AreaLN().ShowArea().All(a => a.IdArea != idArea))
                        {
                            await botClient.SendMessage(chatId, "Número de área inválido. Intenta de nuevo.");
                            return;
                        }
                        estado.IdArea = idArea;
                        estado.Paso = 2;
                        await botClient.SendMessage(chatId, "¿Cuál es el tipo de incidencia? (ej: Hardware, Software, Red)");
                        break;

                    case 2: // esperando tipo de incidencia
                        if (string.IsNullOrWhiteSpace(texto))
                        {
                            await botClient.SendMessage(chatId, "El tipo no puede estar vacío. Intenta de nuevo.");
                            return;
                        }
                        estado.TipoIncidencia = texto;
                        estado.Paso = 3;
                        await botClient.SendMessage(chatId, "Describe el problema (mínimo 10 caracteres):");
                        break;

                    case 3: // esperando descripción
                        if (texto.Trim().Length < 10)
                        {
                            await botClient.SendMessage(chatId, "La descripción debe tener al menos 10 caracteres. Intenta de nuevo.");
                            return;
                        }
                        estado.Descripcion = texto;
                        estado.Paso = 4;
                        await botClient.SendMessage(chatId, "¿Qué prioridad tiene? Responde con el número:\n\n1) Alta\n2) Media\n3) Baja");
                        break;

                    case 4: // esperando prioridad
                        var prioridades = new PrioridadLN().ShowPrioridad();
                        if (!int.TryParse(texto, out int idPrioridad) || prioridades.All(p => p.IdPrioridad != idPrioridad))
                        {
                            await botClient.SendMessage(chatId, "Número de prioridad inválido. Intenta de nuevo.");
                            return;
                        }
                        estado.IdPrioridad = idPrioridad;

                        await FinalizarReportar(chatId, estado);
                        conversaciones.Remove(chatId);
                        break;
                }
            }
            catch (Exception ex)
            {
                conversaciones.Remove(chatId);
                await botClient.SendMessage(chatId, $"Ocurrió un error, el reporte se canceló: {ex.Message}");
            }
        }

        static async Task FinalizarReportar(long chatId, EstadoReportar estado)
        {
            Usuario usuario = new UsuarioLN().BuscarPorChatId(chatId);

            Incidencia nueva = new Incidencia(
                0, null, DateTime.Now,
                $"{usuario.Nombre} {usuario.Apellido}",
                estado.IdArea.Value,
                estado.TipoIncidencia,
                estado.Descripcion,
                estado.IdPrioridad.Value,
                0, // el estado real lo asigna InsertIncidencia (Pendiente)
                null, null, null
            );

            new IncidenciaLN().InsertIncidencia(nueva);

            await botClient.SendMessage(chatId,
                "✅ Incidencia reportada correctamente. Un técnico la atenderá pronto.");
        }

        // ---------- /estado ----------

        static async Task ManejarEstado(long chatId, string[] partes)
        {
            if (partes.Length != 2)
            {
                await botClient.SendMessage(chatId, "Uso: /estado Solicitud-0001");
                return;
            }

            try
            {
                Incidencia incidencia = new IncidenciaLN().BuscarPorTicket(partes[1]);

                if (incidencia == null)
                {
                    await botClient.SendMessage(chatId, $"No se encontró ningún ticket con el número {partes[1]}.");
                    return;
                }

                string tecnico = string.IsNullOrWhiteSpace(incidencia.TecnicoAsignado) ? "Sin asignar" : incidencia.TecnicoAsignado;
                string fechaSolucion = incidencia.FechaSolucion.HasValue ? incidencia.FechaSolucion.Value.ToString("dd/MM/yyyy HH:mm") : "N/A";

                string mensaje =
                    $"🎫 {incidencia.NumeroTicket}\n\n" +
                    $"Estado: {incidencia.NombreEstado}\n" +
                    $"Área: {incidencia.NombreArea}\n" +
                    $"Tipo: {incidencia.TipoIncidencia}\n" +
                    $"Prioridad: {incidencia.NombrePrioridad}\n" +
                    $"Técnico: {tecnico}\n" +
                    $"Fecha creación: {incidencia.Fecha:dd/MM/yyyy HH:mm}\n" +
                    $"Fecha solución: {fechaSolucion}";

                await botClient.SendMessage(chatId, mensaje);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(chatId, $"Ocurrió un error al consultar el ticket: {ex.Message}");
            }
        }
    }

    class EstadoReportar
    {
        public int Paso { get; set; }
        public int? IdArea { get; set; }
        public string TipoIncidencia { get; set; }
        public string Descripcion { get; set; }
        public int? IdPrioridad { get; set; }
    }
}