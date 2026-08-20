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
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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

                // Configuramos el bot para atrapar TODO (Mensajes y clics de botones)
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                };

                // Iniciamos la recepción continua (Polling)
                botClient.StartReceiving(
                    updateHandler: HandleUpdateAsync,
                    errorHandler: HandleErrorAsync,
                    receiverOptions: receiverOptions,
                    cancellationToken: cts.Token
                );

                Console.WriteLine($"Bot iniciado: @{me.Username}");
                Console.WriteLine("Presiona ENTER para detener...");
                Console.ReadLine();

                cts.Cancel();
            }
        }

        // ==========================================
        // ENRUTADORES CENTRALES
        // ==========================================
        static async Task HandleUpdateAsync(ITelegramBotClient cliente, Update update, CancellationToken ct)
        {
            try
            {
                // 1. Si el usuario ESCRIBE un texto
                if (update.Type == UpdateType.Message && update.Message?.Text != null)
                {
                    await OnMessage(update.Message, update.Type);
                }
                // 2. Si el usuario TOCA un botón Inline
                else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
                {
                    await OnCallbackQuery(update.CallbackQuery);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando actualización: {ex.Message}");
            }
        }

        static Task HandleErrorAsync(ITelegramBotClient cliente, Exception exception, CancellationToken ct)
        {
            Console.WriteLine($"Error de API de Telegram: {exception.Message}");
            return Task.CompletedTask;
        }

        // ==========================================
        // EVENTOS (MENSAJES Y BOTONES)
        // ==========================================
        static async Task OnMessage(Message msg, UpdateType type)
        {
            long chatId = msg.Chat.Id;
            string texto = msg.Text.Trim();

            Console.WriteLine($"[{chatId}] Escribió: {texto}");

            // Si hay un reporte en curso, el texto se procesa para la conversación
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

        static async Task OnCallbackQuery(CallbackQuery callbackQuery)
        {
            long chatId = callbackQuery.Message.Chat.Id;
            string datosBoton = callbackQuery.Data;

            // Apaga el icono de "carga" en el botón presionado
            await botClient.AnswerCallbackQuery(callbackQuery.Id);

            if (datosBoton == "cancelar")
            {
                if (conversaciones.ContainsKey(chatId)) conversaciones.Remove(chatId);
                await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId, "❌ Reporte cancelado.");
                return;
            }

            if (conversaciones.ContainsKey(chatId))
            {
                EstadoReportar estado = conversaciones[chatId];

                try
                {
                    // --- PASO 1: Procesar botón de ÁREA ---
                    if (estado.Paso == 1 && datosBoton.StartsWith("area_"))
                    {
                        estado.IdArea = int.Parse(datosBoton.Split('_')[1]);
                        estado.Paso = 2; // Avanzamos a pedir el tipo

                        // Ocultamos los botones y actualizamos el mensaje
                        await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId, "✅ Área seleccionada.");

                        // Crear botones fijos para el Tipo de Incidencia
                        var botonesTipo = new List<InlineKeyboardButton[]>
                        {
                            new[] { InlineKeyboardButton.WithCallbackData("💻 Hardware", "tipo_Hardware") },
                            new[] { InlineKeyboardButton.WithCallbackData("📀 Software", "tipo_Software") },
                            new[] { InlineKeyboardButton.WithCallbackData("🌐 Red", "tipo_Red") },
                            new[] { InlineKeyboardButton.WithCallbackData("➕ Otro", "tipo_Otro") },
                            new[] { InlineKeyboardButton.WithCallbackData("❌ Cancelar", "cancelar") }
                        };

                        await botClient.SendMessage(
                            chatId: chatId,
                            text: "¿Cuál es el tipo de incidencia? *(Toca un botón)*:",
                            parseMode: ParseMode.Markdown,
                            replyMarkup: new InlineKeyboardMarkup(botonesTipo)
                        );
                    }
                    // --- PASO 2: Procesar botón de TIPO ---
                    else if (estado.Paso == 2 && datosBoton.StartsWith("tipo_"))
                    {
                        estado.TipoIncidencia = datosBoton.Substring(5); // Quita "tipo_"
                        estado.Paso = 3; // Avanzamos a pedir la descripción (texto libre)

                        await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId, $"✅ Tipo seleccionado: {estado.TipoIncidencia}");
                        await botClient.SendMessage(chatId, "Describe el problema (mínimo 10 caracteres):");
                    }
                    // --- PASO 4: Procesar botón de PRIORIDAD ---
                    else if (estado.Paso == 4 && datosBoton.StartsWith("pri_"))
                    {
                        estado.IdPrioridad = int.Parse(datosBoton.Split('_')[1]);

                        await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId, "✅ Prioridad seleccionada.");

                        // Todo listo, guardamos en base de datos
                        await FinalizarReportar(chatId, estado);
                        conversaciones.Remove(chatId);
                    }
                }
                catch (Exception ex)
                {
                    conversaciones.Remove(chatId);
                    await botClient.SendMessage(chatId, $"Ocurrió un error, el reporte se canceló: {ex.Message}");
                }
            }
        }

        // ==========================================
        // MÉTODOS DE FLUJO (/reportar)
        // ==========================================
        static async Task IniciarReportar(long chatId)
        {
            try
            {
                Usuario usuario = new UsuarioLN().BuscarPorChatId(chatId);

                if (usuario == null)
                {
                    await botClient.SendMessage(chatId, "Primero debes vincular tu cuenta con /registrar usuario contraseña.");
                    return;
                }

                var areas = new AreaLN().ShowArea();
                var botones = new List<InlineKeyboardButton[]>();

                foreach (var area in areas)
                {
                    botones.Add(new[] { InlineKeyboardButton.WithCallbackData(area.NombreArea, $"area_{area.IdArea}") });
                }
                botones.Add(new[] { InlineKeyboardButton.WithCallbackData("❌ Cancelar", "cancelar") });

                conversaciones[chatId] = new EstadoReportar { Paso = 1 };

                await botClient.SendMessage(
                    chatId: chatId,
                    text: "📝 *Vamos a reportar una incidencia.*\n\n¿En qué área ocurrió? *(Toca un botón)*:",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: new InlineKeyboardMarkup(botones)
                );
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
                    case 3: // Esperando descripción (Texto libre)
                        if (texto.Trim().Length < 10)
                        {
                            await botClient.SendMessage(chatId, "La descripción debe tener al menos 10 caracteres. Intenta de nuevo.");
                            return;
                        }

                        estado.Descripcion = texto;
                        estado.Paso = 4; // Pasamos a preguntar la prioridad

                        var prioridades = new PrioridadLN().ShowPrioridad();
                        var botonesPri = new List<InlineKeyboardButton[]>();

                        foreach (var pri in prioridades)
                        {
                            botonesPri.Add(new[] { InlineKeyboardButton.WithCallbackData(pri.Nombre, $"pri_{pri.IdPrioridad}") });
                        }
                        botonesPri.Add(new[] { InlineKeyboardButton.WithCallbackData("❌ Cancelar", "cancelar") });

                        await botClient.SendMessage(
                            chatId: chatId,
                            text: "¿Qué prioridad tiene? *(Toca un botón)*:",
                            parseMode: ParseMode.Markdown,
                            replyMarkup: new InlineKeyboardMarkup(botonesPri)
                        );
                        break;

                    default:
                        await botClient.SendMessage(chatId, "Por favor, selecciona una opción de los botones o escribe /cancelar.");
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
            string nombreEmpleado = $"{usuario.Nombre} {usuario.Apellido}";

            Incidencia nueva = new Incidencia(
                0, null, DateTime.Now,
                nombreEmpleado,
                estado.IdArea.Value,
                estado.TipoIncidencia,
                estado.Descripcion,
                estado.IdPrioridad.Value,
                0, // El estado real (Pendiente) lo asigna InsertIncidencia
                null, null, null
            );

            // 1. Insertamos la incidencia
            new IncidenciaLN().InsertIncidencia(nueva);

            // 2. Buscamos la última incidencia insertada por este usuario para obtener el ticket
            Incidencia ultimaInsertada = new IncidenciaLN().ShowIncidencia()
                .Where(i => i.Empleado == nombreEmpleado)
                .OrderByDescending(i => i.IdIncidencia)
                .FirstOrDefault();

            string numeroTicket = ultimaInsertada != null ? ultimaInsertada.NumeroTicket : "tu solicitud";

            // 3. Enviamos el mensaje personalizado
            await botClient.SendMessage(chatId, $"✅ {numeroTicket} reportada correctamente. Un técnico la atenderá pronto.");

            // 4. NUEVO: Notificamos a todos los técnicos
            if (ultimaInsertada != null)
            {
                var tecnicos = new UsuarioLN().ShowUsuario()
                    .Where(u => u.Rol == "Técnico" && u.Estado && u.TelegramChatId.HasValue)
                    .ToList();

                string mensajeTecnicos = $"🚨 *NUEVA INCIDENCIA DESDE TELEGRAM* 🚨\n\n" +
                                         $"*Ticket:* {numeroTicket}\n" +
                                         $"*Empleado:* {ultimaInsertada.Empleado}\n" +
                                         $"*Área:* {ultimaInsertada.NombreArea}\n" +
                                         $"*Tipo:* {ultimaInsertada.TipoIncidencia}\n" +
                                         $"*Prioridad:* {ultimaInsertada.NombrePrioridad}\n\n" +
                                         $"*Descripción:*\n{ultimaInsertada.Descripcion}";

                foreach (var tecnico in tecnicos)
                {
                    try
                    {
                        // Usamos TelegramNotificador para enviar el mensaje silenciosamente a cada uno
                        TelegramNotificador.EnviarMensaje(tecnico.TelegramChatId.Value, mensajeTecnicos);
                    }
                    catch { /* Ignorar si falla un técnico específico */ }
                }
            }
        }

        // ==========================================
        // OTROS COMANDOS
        // ==========================================
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
                bool ok = new UsuarioLN().RegistrarChatTelegram(usuario, password, msg.Chat.Id);
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

        static async Task ManejarEstado(long chatId, string[] partes)
        {
            try
            {
                Usuario usuario = new UsuarioLN().BuscarPorChatId(chatId);

                if (usuario == null)
                {
                    await botClient.SendMessage(chatId, "Primero debes vincular tu cuenta con /registrar usuario contraseña.");
                    return;
                }

                Incidencia incidencia = null;

                // Si solo escribe "/estado" (partes.Length == 1)
                if (partes.Length == 1)
                {
                    string nombreEmpleado = $"{usuario.Nombre} {usuario.Apellido}";

                    incidencia = new IncidenciaLN().ShowIncidencia()
                        .Where(i => i.Empleado == nombreEmpleado)
                        .OrderByDescending(i => i.IdIncidencia)
                        .FirstOrDefault();

                    if (incidencia == null)
                    {
                        await botClient.SendMessage(chatId, "No tienes ninguna incidencia reportada aún.");
                        return;
                    }
                }
                // Si escribe "/estado Solicitud-0001" (partes.Length == 2)
                else if (partes.Length == 2)
                {
                    incidencia = new IncidenciaLN().BuscarPorTicket(partes[1]);

                    if (incidencia == null)
                    {
                        await botClient.SendMessage(chatId, $"No se encontró ningún ticket con el número {partes[1]}.");
                        return;
                    }
                }
                else
                {
                    await botClient.SendMessage(chatId, "Uso rápido: /estado\nPara una específica: /estado Solicitud-0001");
                    return;
                }

                // Construir y enviar el mensaje
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

        // ==========================================
        // CLASE AUXILIAR
        // ==========================================
        class EstadoReportar
        {
            public int Paso { get; set; }
            public int? IdArea { get; set; }
            public string TipoIncidencia { get; set; }
            public string Descripcion { get; set; }
            public int? IdPrioridad { get; set; }
        }
    }
}