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

            // --- 1. PROCESAR BOTÓN DE ACEPTAR TICKET ---
            if (datosBoton.StartsWith("aceptar_"))
            {
                int idIncidencia = int.Parse(datosBoton.Split('_')[1]);
                var logica = new Logica.Gestion_de_Logica.IncidenciaLN();
                string resultado = logica.AceptarIncidenciaPorTelegram(idIncidencia, chatId);

                if (resultado == "SUCCESS")
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id, "¡Ticket asignado a ti correctamente!", showAlert: false);

                    // 1. Reconstruimos el texto original consultando la BD
                    // (Así no perdemos los asteriscos y formato al editar el mensaje de los demás)
                    var inc = logica.BuscarPorTicket(logica.ShowIncidencia().FirstOrDefault(i => i.IdIncidencia == idIncidencia)?.NumeroTicket);
                    string textoBase = $"🚨 *NUEVA INCIDENCIA REPORTADA* 🚨\n\n*Ticket:* {inc.NumeroTicket}\n*Empleado:* {inc.Empleado}\n*Área:* {inc.NombreArea}\n*Tipo:* {inc.TipoIncidencia}\n*Prioridad:* {inc.NombrePrioridad}\n\n*Descripción:*\n{inc.Descripcion}";

                    // 2. Traemos de SQL Server TODOS los chats que recibieron esta alerta
                    var mensajesEnviados = logica.ObtenerMensajesTelegram(idIncidencia);

                    var botonesEstado = new InlineKeyboardMarkup(new[] {
                        new[] { InlineKeyboardButton.WithCallbackData("✅ Marcar Resuelto", $"estado_{idIncidencia}_Resuelto") },
                        new[] { InlineKeyboardButton.WithCallbackData("🔒 Cerrar Ticket", $"estado_{idIncidencia}_Cerrado") }
                    });

                    // 3. ¡EL BARRIDO MÁGICO! Editamos el chat de todos los técnicos al mismo tiempo
                    foreach (var m in mensajesEnviados)
                    {
                        try
                        {
                            if (m.ChatId == chatId)
                            {
                                // Para el ganador: Le mostramos que lo aceptó y le damos los botones de estado
                                await botClient.EditMessageText(
                                    chatId: m.ChatId,
                                    messageId: m.MessageId,
                                    text: textoBase + "\n\n👉 *¡Aceptaste este ticket y está En Proceso!*",
                                    parseMode: ParseMode.Markdown,
                                    replyMarkup: botonesEstado
                                );
                            }
                            else
                            {
                                // Para los demás: Borramos el botón (replyMarkup: null) y les avisamos que ya lo tomaron
                                await botClient.EditMessageText(
                                    chatId: m.ChatId,
                                    messageId: m.MessageId,
                                    text: textoBase + $"\n\n🔒 *(Este ticket fue tomado por otro técnico)*",
                                    parseMode: ParseMode.Markdown,
                                    replyMarkup: null
                                );
                            }
                        }
                        catch { /* Ignoramos si alguien borró el chat o bloqueó al bot */ }
                    }
                }
                else
                {
                    // Si el técnico le dio clic pero alguien se le adelantó una fracción de segundo:
                    await botClient.AnswerCallbackQuery(callbackQuery.Id, resultado, showAlert: true);

                    // Le quitamos el botón obsoleto
                    await botClient.EditMessageText(
                        chatId: chatId,
                        messageId: callbackQuery.Message.MessageId,
                        text: callbackQuery.Message.Text + "\n\n🔒 *(Ticket tomado por otro técnico)*"
                    );
                }
                return; // Salimos para no seguir evaluando
            }

            // --- 2. PROCESAR BOTONES DE CAMBIO DE ESTADO ---
            if (datosBoton.StartsWith("estado_"))
            {
                string[] partes = datosBoton.Split('_');
                int idIncidencia = int.Parse(partes[1]);
                string nuevoEstado = partes[2]; // "Resuelto" o "Cerrado"

                string resultado = new Logica.Gestion_de_Logica.IncidenciaLN().CambiarEstadoPorTelegram(idIncidencia, chatId, nuevoEstado);

                if (resultado.StartsWith("SUCCESS"))
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id, $"Ticket {nuevoEstado} exitosamente.", showAlert: false);

                    // Quitamos los botones y dejamos el mensaje final
                    await botClient.EditMessageText(
                        chatId: chatId,
                        messageId: callbackQuery.Message.MessageId,
                        text: callbackQuery.Message.Text + $"\n\n🏁 *Ticket {nuevoEstado} por ti.*",
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
                    );
                }
                else
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id, resultado, showAlert: true);
                }
                return; // Salimos para no seguir evaluando
            }

            // --- 3. CÓDIGO EXISTENTE DE CREACIÓN DE TICKETS ---
            // Apaga el icono de "carga" en cualquier otro botón presionado
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
                    // Procesar botón de ÁREA
                    if (estado.Paso == 1 && datosBoton.StartsWith("area_"))
                    {
                        estado.IdArea = int.Parse(datosBoton.Split('_')[1]);
                        estado.Paso = 2;
                        await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId, "✅ Área seleccionada.");

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
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                            replyMarkup: new InlineKeyboardMarkup(botonesTipo)
                        );
                    }
                    // Procesar botón de TIPO
                    else if (estado.Paso == 2 && datosBoton.StartsWith("tipo_"))
                    {
                        estado.TipoIncidencia = datosBoton.Substring(5);
                        estado.Paso = 3;

                        await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId, $"✅ Tipo seleccionado: {estado.TipoIncidencia}");
                        await botClient.SendMessage(chatId, "Describe el problema (mínimo 10 caracteres):");
                    }
                    // Procesar botón de PRIORIDAD
                    else if (estado.Paso == 4 && datosBoton.StartsWith("pri_"))
                    {
                        estado.IdPrioridad = int.Parse(datosBoton.Split('_')[1]);
                        await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId, "✅ Prioridad seleccionada.");

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
                        int? messageId = Bot.TelegramNotificador.EnviarMensaje(tecnico.TelegramChatId.Value, mensajeTecnicos, ultimaInsertada.IdIncidencia);

                        if (messageId.HasValue)
                        {
                            new Logica.Gestion_de_Logica.IncidenciaLN().RegistrarMensajeTelegram(ultimaInsertada.IdIncidencia, tecnico.TelegramChatId.Value, messageId.Value);
                        }
                    }
                    catch { }
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