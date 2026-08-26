using Entidades.Gestion_de_Entidades;
using Logica;
using Logica.Gestion_de_Logica;
using Serilog; // NUEVO: Librería de Logs
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Topshelf;

namespace Bot
{
    class Program
    {
        static TelegramBotClient botClient;
        private static readonly ConcurrentDictionary<long, EstadoReportar> conversaciones = new ConcurrentDictionary<long, EstadoReportar>();
        private static readonly ConcurrentDictionary<long, EstadoTecnico> conversacionesTecnicos = new ConcurrentDictionary<long, EstadoTecnico>();
        static CancellationTokenSource cts;

        static void Main(string[] args)
        {
            // 1. Configuración del Sistema de Logs
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console() // Muestra los logs en la consola local
                .WriteTo.File("logs\\bot_.txt",
                    rollingInterval: RollingInterval.Day, // Un archivo nuevo cada día
                    retainedFileCountLimit: 30) // Guarda máximo 30 días para no llenar el disco
                .CreateLogger();

            try
            {
                Log.Information("Iniciando el ejecutable del Bot...");

                var rc = HostFactory.Run(x =>
                {
                    // Le decimos a TopShelf que envíe sus reportes a nuestro archivo de Serilog
                    x.UseSerilog();

                    x.Service<BotMotor>(s =>
                    {
                        s.ConstructUsing(name => new BotMotor());
                        s.WhenStarted(tc => tc.Start());
                        s.WhenStopped(tc => tc.Stop());
                    });

                    x.RunAsLocalSystem();
                    x.SetDescription("Servicio en segundo plano del Bot de Telegram para el Sistema de Incidencias");
                    x.SetDisplayName("Incidencias Telegram Bot");
                    x.SetServiceName("IncidenciasTelegramBot");
                });

                var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
                Environment.ExitCode = exitCode;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "El servicio del Bot colapsó catastróficamente.");
            }
            finally
            {
                // Asegura que todos los textos se guarden en el archivo antes de cerrar
                Log.CloseAndFlush();
            }
        }

        public class BotMotor
        {
            public void Start()
            {
                string token = ConfigurationManager.AppSettings["TelegramBotToken"];
                cts = new CancellationTokenSource();
                botClient = new TelegramBotClient(token, cancellationToken: cts.Token);

                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                };

                botClient.StartReceiving(
                    updateHandler: HandleUpdateAsync,
                    errorHandler: HandleErrorAsync,
                    receiverOptions: receiverOptions,
                    cancellationToken: cts.Token
                );

                Log.Information("Bot de Telegram conectado y escuchando mensajes correctamente.");
            }

            public void Stop()
            {
                cts?.Cancel();
                Log.Information("El Bot fue detenido de forma segura.");
            }
        }

        // ==========================================
        // ENRUTADORES CENTRALES
        // ==========================================
        static async Task HandleUpdateAsync(ITelegramBotClient cliente, Update update, CancellationToken ct)
        {
            try
            {
                if (update.Type == UpdateType.Message && update.Message?.Text != null)
                {
                    await OnMessage(update.Message, update.Type);
                }
                else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
                {
                    await OnCallbackQuery(update.CallbackQuery);
                }
            }
            catch (Exception ex)
            {
                // Guardamos el error real en el archivo de texto
                Log.Error(ex, "Error crítico procesando una actualización de Telegram.");
            }
        }

        static Task HandleErrorAsync(ITelegramBotClient cliente, Exception exception, CancellationToken ct)
        {
            Log.Error(exception, "Error de comunicación con la API de Telegram.");
            return Task.CompletedTask;
        }

        // ==========================================
        // EVENTOS (MENSAJES Y BOTONES)
        // ==========================================
        static async Task OnMessage(Message msg, UpdateType type)
        {
            long chatId = msg.Chat.Id;
            string texto = msg.Text.Trim();

            Log.Information("El chat [{ChatId}] envió el comando/mensaje: {Texto}", chatId, texto);

            if (conversaciones.TryGetValue(chatId, out _))
            {
                if (texto.ToLower() == "/cancelar")
                {
                    conversaciones.TryRemove(chatId, out _);
                    await botClient.SendMessage(chatId, "❌ Reporte cancelado.");
                    return;
                }

                await ContinuarReportar(chatId, texto);
                return;
            }

            if (conversacionesTecnicos.TryGetValue(chatId, out var estadoTec))
            {
                await FinalizarEstadoTicket(chatId, estadoTec.IdIncidencia, estadoTec.NuevoEstado, texto);
                conversacionesTecnicos.TryRemove(chatId, out _);
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

            Log.Information("El chat [{ChatId}] presionó el botón interactivo: {DatosBoton}", chatId, datosBoton);

            // --- 1. PROCESAR BOTÓN DE ACEPTAR TICKET ---
            if (datosBoton.StartsWith("aceptar_"))
            {
                int idIncidencia = int.Parse(datosBoton.Split('_')[1]);
                var logica = new Logica.Gestion_de_Logica.IncidenciaLN();
                string resultado = logica.AceptarIncidenciaPorTelegram(idIncidencia, chatId);

                if (resultado == "SUCCESS")
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id, "¡Ticket asignado a ti correctamente!", showAlert: false);

                    var inc = logica.BuscarPorTicket(logica.ShowIncidencia().FirstOrDefault(i => i.IdIncidencia == idIncidencia)?.NumeroTicket);
                    string textoBase = $"🚨 *NUEVA INCIDENCIA REPORTADA* 🚨\n\n*Ticket:* {inc.NumeroTicket}\n*Empleado:* {inc.Empleado}\n*Área:* {inc.NombreArea}\n*Tipo:* {inc.TipoIncidencia}\n*Prioridad:* {inc.NombrePrioridad}\n\n*Descripción:*\n{inc.Descripcion}";

                    var mensajesEnviados = logica.ObtenerMensajesTelegram(idIncidencia);

                    var botonesEstado = new InlineKeyboardMarkup(new[] {
                        new[] { InlineKeyboardButton.WithCallbackData("✅ Marcar Resuelto", $"estado_{idIncidencia}_Resuelto") },
                        new[] { InlineKeyboardButton.WithCallbackData("🔒 Cerrar Ticket", $"estado_{idIncidencia}_Cerrado") }
                    });

                    foreach (var m in mensajesEnviados)
                    {
                        try
                        {
                            if (m.ChatId == chatId)
                            {
                                await botClient.EditMessageText(chatId: m.ChatId, messageId: m.MessageId, text: textoBase + "\n\n👉 *¡Aceptaste este ticket y está En Proceso!*", parseMode: ParseMode.Markdown, replyMarkup: botonesEstado);
                            }
                            else
                            {
                                await botClient.EditMessageText(chatId: m.ChatId, messageId: m.MessageId, text: textoBase + $"\n\n🔒 *(Este ticket fue tomado por otro técnico)*", parseMode: ParseMode.Markdown, replyMarkup: null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, "Error borrando el botón al técnico con ChatId {ChatId}", m.ChatId);
                        }
                    }
                }
                else
                {
                    await botClient.AnswerCallbackQuery(callbackQuery.Id, resultado, showAlert: true);
                    await botClient.EditMessageText(chatId: chatId, messageId: callbackQuery.Message.MessageId, text: callbackQuery.Message.Text + "\n\n🔒 *(Ticket tomado por otro técnico)*");
                }
                return;
            }

            // --- 2. PROCESAR BOTONES DE CAMBIO DE ESTADO ---
            if (datosBoton.StartsWith("estado_"))
            {
                string[] partes = datosBoton.Split('_');
                int idIncidencia = int.Parse(partes[1]);
                string nuevoEstado = partes[2]; // "Resuelto" o "Cerrado"

                // 1. Guardamos el estado temporalmente
                conversacionesTecnicos[chatId] = new EstadoTecnico { IdIncidencia = idIncidencia, NuevoEstado = nuevoEstado };

                // 2. Le damos un botón por si no quiere escribir nada
                var botonOmitir = new InlineKeyboardMarkup(new[] {
        InlineKeyboardButton.WithCallbackData("⏭️ Omitir observación", "omitir_obs")
    });

                // 3. Le pedimos que escriba
                await botClient.SendMessage(chatId, $"Has elegido marcar el ticket como *{nuevoEstado}*.\n\nPor favor, escribe una observación sobre el trabajo realizado:", parseMode: ParseMode.Markdown, replyMarkup: botonOmitir);

                await botClient.AnswerCallbackQuery(callbackQuery.Id);
                return;
            }

            // NUEVO: Manejar el botón de "Omitir"
            if (datosBoton == "omitir_obs")
            {
                if (conversacionesTecnicos.TryGetValue(chatId, out var estadoTecOmitir))
                {
                    await FinalizarEstadoTicket(chatId, estadoTecOmitir.IdIncidencia, estadoTecOmitir.NuevoEstado, "Sin observaciones detalladas.");
                    conversacionesTecnicos.TryRemove(chatId, out _);
                }
                await botClient.AnswerCallbackQuery(callbackQuery.Id);
                // Borramos el mensaje de "escribe una observación"
                await botClient.DeleteMessage(chatId, callbackQuery.Message.MessageId);
                return;
            }

            // --- 3. CÓDIGO EXISTENTE DE CREACIÓN DE TICKETS ---
            await botClient.AnswerCallbackQuery(callbackQuery.Id);

            if (datosBoton == "cancelar")
            {
                conversaciones.TryRemove(chatId, out _);
                await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId, "❌ Reporte cancelado.");
                return;
            }

            if (conversaciones.TryGetValue(chatId, out var estado))
            {
                try
                {
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

                        await botClient.SendMessage(chatId: chatId, text: "¿Cuál es el tipo de incidencia? *(Toca un botón)*:", parseMode: ParseMode.Markdown, replyMarkup: new InlineKeyboardMarkup(botonesTipo));
                    }
                    else if (estado.Paso == 2 && datosBoton.StartsWith("tipo_"))
                    {
                        estado.TipoIncidencia = datosBoton.Substring(5);
                        estado.Paso = 3;

                        await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId, $"✅ Tipo seleccionado: {estado.TipoIncidencia}");
                        await botClient.SendMessage(chatId, "Describe el problema (mínimo 10 caracteres):");
                    }
                    else if (estado.Paso == 4 && datosBoton.StartsWith("pri_"))
                    {
                        estado.IdPrioridad = int.Parse(datosBoton.Split('_')[1]);
                        await botClient.EditMessageText(chatId, callbackQuery.Message.MessageId, "✅ Prioridad seleccionada.");

                        await FinalizarReportar(chatId, estado);
                        conversaciones.TryRemove(chatId, out _);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error durante el flujo interactivo de reporte.");
                    conversaciones.TryRemove(chatId, out _);
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

                await botClient.SendMessage(chatId: chatId, text: "📝 *Vamos a reportar una incidencia.*\n\n¿En qué área ocurrió? *(Toca un botón)*:", parseMode: ParseMode.Markdown, replyMarkup: new InlineKeyboardMarkup(botones));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error en IniciarReportar");
                await botClient.SendMessage(chatId, $"Error al iniciar el reporte: {ex.Message}");
            }
        }

        static async Task ContinuarReportar(long chatId, string texto)
        {
            if (!conversaciones.TryGetValue(chatId, out var estado))
            {
                await botClient.SendMessage(chatId, "No hay un reporte en curso. Escribe /reportar para iniciar uno.");
                return;
            }

            try
            {
                switch (estado.Paso)
                {
                    case 3:
                        if (texto.Trim().Length < 10)
                        {
                            await botClient.SendMessage(chatId, "La descripción debe tener al menos 10 caracteres. Intenta de nuevo.");
                            return;
                        }

                        estado.Descripcion = texto;
                        estado.Paso = 4;

                        var prioridades = new PrioridadLN().ShowPrioridad();
                        var botonesPri = new List<InlineKeyboardButton[]>();

                        foreach (var pri in prioridades)
                        {
                            botonesPri.Add(new[] { InlineKeyboardButton.WithCallbackData(pri.Nombre, $"pri_{pri.IdPrioridad}") });
                        }
                        botonesPri.Add(new[] { InlineKeyboardButton.WithCallbackData("❌ Cancelar", "cancelar") });

                        await botClient.SendMessage(chatId: chatId, text: "¿Qué prioridad tiene? *(Toca un botón)*:", parseMode: ParseMode.Markdown, replyMarkup: new InlineKeyboardMarkup(botonesPri));
                        break;

                    default:
                        await botClient.SendMessage(chatId, "Por favor, selecciona una opción de los botones o escribe /cancelar.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error en ContinuarReportar");
                conversaciones.TryRemove(chatId, out _);
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
                0,
                null, null, null
            );

            new IncidenciaLN().InsertIncidencia(nueva, out int idIncidencia);

            Incidencia ultimaInsertada = new IncidenciaLN().ShowIncidencia()
        .FirstOrDefault(i => i.IdIncidencia == idIncidencia);

            string numeroTicket = ultimaInsertada != null ? ultimaInsertada.NumeroTicket : "tu solicitud";

            await botClient.SendMessage(chatId, $"✅ {numeroTicket} reportada correctamente. Un técnico la atenderá pronto.");

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
                        int? messageId = await Bot.TelegramNotificador.EnviarMensajeAsync(tecnico.TelegramChatId.Value, mensajeTecnicos, ultimaInsertada.IdIncidencia);

                        if (messageId.HasValue)
                        {
                            new Logica.Gestion_de_Logica.IncidenciaLN().RegistrarMensajeTelegram(ultimaInsertada.IdIncidencia, tecnico.TelegramChatId.Value, messageId.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error enviando notificación al técnico {Tecnico}", tecnico.Nombre);
                    }
                }
            }
        }

        static async Task FinalizarEstadoTicket(long chatId, int idIncidencia, string nuevoEstado, string observacion)
        {
            try
            {
                // ATENCIÓN AQUÍ: Tu método CambiarEstadoPorTelegram ahora deberá recibir la 'observacion'
                string resultado = new Logica.Gestion_de_Logica.IncidenciaLN().CambiarEstadoPorTelegram(idIncidencia, chatId, nuevoEstado, observacion);

                if (resultado.StartsWith("SUCCESS"))
                {
                    await botClient.SendMessage(chatId, $"🏁 *Ticket {nuevoEstado} exitosamente.*\n\n*Observación guardada:*\n_{observacion}_", parseMode: ParseMode.Markdown);
                    Log.Information("Técnico [{ChatId}] cambió ticket {Id} a {Estado} con obs: {Obs}", chatId, idIncidencia, nuevoEstado, observacion);
                }
                else
                {
                    await botClient.SendMessage(chatId, $"❌ No se pudo actualizar el ticket: {resultado}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al guardar observación del técnico");
                await botClient.SendMessage(chatId, "Ocurrió un error al intentar guardar el ticket.");
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
                    Log.Information("Usuario {Usuario} vinculó su cuenta de Telegram con éxito.", usuario);
                }
            }
            catch (LogicaExcepciones ex)
            {
                await botClient.SendMessage(msg.Chat, $"No se pudo vincular: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error inesperado al intentar registrar cuenta de Telegram.");
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
                Log.Error(ex, "Error al manejar el comando /estado");
                await botClient.SendMessage(chatId, $"Ocurrió un error al consultar el ticket: {ex.Message}");
            }
        }

        // ==========================================2
        // CLASES AUXILIARES
        // ==========================================
        class EstadoReportar
        {
            public int Paso { get; set; }
            public int? IdArea { get; set; }
            public string TipoIncidencia { get; set; }
            public string Descripcion { get; set; }
            public int? IdPrioridad { get; set; }
        }

        class EstadoTecnico
        {
            public int IdIncidencia { get; set; }
            public string NuevoEstado { get; set; }
        }
    }
}