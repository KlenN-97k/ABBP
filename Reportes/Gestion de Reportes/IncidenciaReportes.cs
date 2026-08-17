using ClosedXML.Excel;
using Entidades.Gestion_de_Entidades;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reportes
{
    public static class IncidenciaReportes
    {
        private static readonly Color ColorNavy = Color.FromRGB(21, 50, 80);
        private static readonly Color ColorAcero = Color.FromRGB(43, 107, 154);
        private static readonly Color ColorNavyClaro = Color.FromRGB(180, 200, 220);
        private static readonly Color ColorFilaPar = Color.FromRGB(245, 246, 250);

        static IncidenciaReportes()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        // ---------- Encabezado y pie reutilizables ----------

        private static void Encabezado(PageDescriptor page, string tituloReporte, int totalRegistros)
        {
            page.Header().Background(ColorNavy).Padding(25).Column(col =>
            {
                col.Item().Text("Sistema de Gestión de Incidencias APPB")
                    .FontColor(Colors.White).FontSize(11).Bold();

                col.Item().PaddingTop(4).Text(tituloReporte)
                    .FontColor(Colors.White).FontSize(20).Bold();

                col.Item().PaddingTop(6).Text($"Generado el {DateTime.Now:dd 'de' MMMM 'de' yyyy, HH:mm}  •  {totalRegistros} registro(s)")
                    .FontColor(ColorNavyClaro).FontSize(9);
            });
        }

        private static void PieDePagina(PageDescriptor page)
        {
            page.Footer().Padding(15).Row(row =>
            {
                row.RelativeItem().Text("Sistema de Gestión de Incidencias - APPB 2027")
                    .FontSize(8).FontColor(Colors.Grey.Medium);

                row.RelativeItem().AlignRight().Text(x =>
                {
                    x.Span("Página ").FontSize(8).FontColor(Colors.Grey.Medium);
                    x.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Medium);
                    x.Span(" de ").FontSize(8).FontColor(Colors.Grey.Medium);
                    x.TotalPages().FontSize(8).FontColor(Colors.Grey.Medium);
                });
            });
        }

        // ---------- Incidencias ----------

        public static byte[] GenerarPdfListado(List<Incidencia> incidencias, string tituloReporte = "Reporte de Incidencias")
        {
            try
            {
                var documento = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4.Landscape());
                        page.Margin(0);
                        page.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                        Encabezado(page, tituloReporte, incidencias.Count);

                        page.Content().Padding(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(75);
                                columns.ConstantColumn(60);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1.6f);
                                columns.RelativeColumn(1.6f);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                void CeldaEncabezado(string texto)
                                {
                                    header.Cell().Element(c => c.Background(ColorAcero).Padding(5))
                                        .Text(texto).FontColor(Colors.White).Bold();
                                }

                                CeldaEncabezado("Ticket");
                                CeldaEncabezado("Fecha");
                                CeldaEncabezado("Empleado");
                                CeldaEncabezado("Área");
                                CeldaEncabezado("Tipo");
                                CeldaEncabezado("Prioridad");
                                CeldaEncabezado("Estado");
                                CeldaEncabezado("Técnico");
                            });

                            int fila = 0;
                            foreach (Incidencia inc in incidencias)
                            {
                                Color fondo = fila % 2 == 0 ? Colors.White : ColorFilaPar;

                                table.Cell().Background(fondo).Padding(4).Text(inc.NumeroTicket);
                                table.Cell().Background(fondo).Padding(4).Text(inc.Fecha.ToString("dd/MM/yyyy"));
                                table.Cell().Background(fondo).Padding(4).Text(inc.Empleado);
                                table.Cell().Background(fondo).Padding(4).Text(inc.NombreArea ?? "-");
                                table.Cell().Background(fondo).Padding(4).Text(inc.TipoIncidencia);
                                table.Cell().Background(fondo).Padding(4).Text(inc.NombrePrioridad ?? "-");
                                table.Cell().Background(fondo).Padding(4).Text(inc.NombreEstado ?? "-");
                                table.Cell().Background(fondo).Padding(4).Text(inc.TecnicoAsignado ?? "Sin asignar");

                                fila++;
                            }
                        });

                        PieDePagina(page);
                    });
                });

                return documento.GeneratePdf();
            }
            catch (Exception ex)
            {
                throw new ReportesExcepciones("Error al generar el PDF de incidencias", ex);
            }
        }

        public static byte[] GenerarExcelListado(List<Incidencia> incidencias)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var hoja = workbook.Worksheets.Add("Incidencias");

                    string[] encabezados =
                    {
                        "Ticket", "Fecha", "Empleado", "Área", "Tipo", "Descripción",
                        "Prioridad", "Estado", "Técnico Asignado", "Fecha Solución", "Observaciones"
                    };

                    for (int i = 0; i < encabezados.Length; i++)
                        hoja.Cell(1, i + 1).Value = encabezados[i];

                    hoja.Row(1).Style.Font.Bold = true;
                    hoja.Row(1).Style.Font.FontColor = XLColor.White;
                    hoja.Row(1).Style.Fill.BackgroundColor = XLColor.FromArgb(21, 50, 80);

                    int fila = 2;
                    foreach (Incidencia inc in incidencias)
                    {
                        hoja.Cell(fila, 1).Value = inc.NumeroTicket;

                        hoja.Cell(fila, 2).Value = inc.Fecha;
                        hoja.Cell(fila, 2).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";

                        hoja.Cell(fila, 3).Value = inc.Empleado;
                        hoja.Cell(fila, 4).Value = inc.NombreArea;
                        hoja.Cell(fila, 5).Value = inc.TipoIncidencia;
                        hoja.Cell(fila, 6).Value = inc.Descripcion;
                        hoja.Cell(fila, 7).Value = inc.NombrePrioridad;
                        hoja.Cell(fila, 8).Value = inc.NombreEstado;
                        hoja.Cell(fila, 9).Value = inc.TecnicoAsignado ?? "Sin asignar";

                        if (inc.FechaSolucion.HasValue)
                        {
                            hoja.Cell(fila, 10).Value = inc.FechaSolucion.Value;
                            hoja.Cell(fila, 10).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                        }

                        hoja.Cell(fila, 11).Value = inc.Observaciones;
                        fila++;
                    }

                    hoja.Columns().AdjustToContents();
                    hoja.SheetView.FreezeRows(1);
                    hoja.RangeUsed().SetAutoFilter();

                    using (var ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ReportesExcepciones("Error al generar el Excel de incidencias", ex);
            }
        }

        // ---------- Métricas y Filtro (sin cambios) ----------

        public static MetricasIncidencias CalcularMetricas(List<Incidencia> incidencias)
        {
            try
            {
                var metricas = new MetricasIncidencias
                {
                    Total = incidencias.Count,
                    PorEstado = incidencias
                        .GroupBy(i => i.NombreEstado ?? "Sin estado")
                        .ToDictionary(g => g.Key, g => g.Count()),
                    PorPrioridad = incidencias
                        .GroupBy(i => i.NombrePrioridad ?? "Sin prioridad")
                        .ToDictionary(g => g.Key, g => g.Count()),
                    PorArea = incidencias
                        .GroupBy(i => i.NombreArea ?? "Sin área")
                        .ToDictionary(g => g.Key, g => g.Count())
                };

                List<Incidencia> resueltas = incidencias.Where(i => i.FechaSolucion.HasValue).ToList();
                if (resueltas.Count > 0)
                {
                    metricas.TiempoPromedioResolucionHoras = resueltas
                        .Average(i => (i.FechaSolucion.Value - i.Fecha).TotalHours);
                }

                return metricas;
            }
            catch (Exception ex)
            {
                throw new ReportesExcepciones("Error al calcular métricas de incidencias", ex);
            }
        }

        public static List<Incidencia> Filtrar(List<Incidencia> incidencias, FiltroIncidencias filtro)
        {
            if (incidencias == null)
                throw new ReportesExcepciones("Debe proporcionar una lista de incidencias.", null);

            if (filtro == null)
                return incidencias;

            try
            {
                IEnumerable<Incidencia> resultado = incidencias;

                if (filtro.FechaDesde.HasValue)
                    resultado = resultado.Where(i => i.Fecha.Date >= filtro.FechaDesde.Value.Date);
                if (filtro.FechaHasta.HasValue)
                    resultado = resultado.Where(i => i.Fecha.Date <= filtro.FechaHasta.Value.Date);
                if (filtro.IdArea.HasValue)
                    resultado = resultado.Where(i => i.IdArea == filtro.IdArea.Value);
                if (filtro.IdPrioridad.HasValue)
                    resultado = resultado.Where(i => i.IdPrioridad == filtro.IdPrioridad.Value);
                if (filtro.IdEstado.HasValue)
                    resultado = resultado.Where(i => i.IdEstado == filtro.IdEstado.Value);
                if (filtro.IdTecnicoAsignado.HasValue)
                    resultado = resultado.Where(i => i.IdTecnicoAsignado == filtro.IdTecnicoAsignado.Value);

                return resultado.ToList();
            }
            catch (Exception ex)
            {
                throw new ReportesExcepciones("Error al filtrar incidencias", ex);
            }
        }

        // ---------- Guías ----------

        public static byte[] GenerarPdfListadoGuias(List<Guia> guias, string tituloReporte = "Catálogo de Guías de Ayuda")
        {
            if (guias == null)
                throw new ReportesExcepciones("Debe proporcionar una lista de guías.", null);

            try
            {
                var documento = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(0);
                        page.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(10));

                        Encabezado(page, tituloReporte, guias.Count);

                        page.Content().Padding(25).Column(col =>
                        {
                            col.Spacing(14);

                            foreach (Guia g in guias)
                            {
                                col.Item().Border(1).BorderColor(Color.FromRGB(220, 225, 230)).Column(tarjeta =>
                                {
                                    tarjeta.Item().Background(ColorAcero).Padding(10)
                                        .Text(g.Titulo).FontColor(Colors.White).FontSize(13).Bold();

                                    tarjeta.Item().Padding(12).Column(cuerpo =>
                                    {
                                        cuerpo.Spacing(6);

                                        cuerpo.Item().Text(t =>
                                        {
                                            t.Span("PROBLEMA  ").FontColor(Color.FromRGB(231, 76, 60)).FontSize(8).Bold();
                                        });
                                        cuerpo.Item().Text(g.Problema).FontSize(10).LineHeight(1.3f);

                                        cuerpo.Item().PaddingTop(4).Text(t =>
                                        {
                                            t.Span("SOLUCIÓN  ").FontColor(Color.FromRGB(39, 174, 96)).FontSize(8).Bold();
                                        });
                                        cuerpo.Item().Text(g.Solucion).FontSize(10).LineHeight(1.3f);
                                    });
                                });
                            }
                        });

                        PieDePagina(page);
                    });
                });

                return documento.GeneratePdf();
            }
            catch (Exception ex)
            {
                throw new ReportesExcepciones("Error al generar el PDF de guías", ex);
            }
        }

        public static byte[] GenerarExcelListadoGuias(List<Guia> guias)
        {
            if (guias == null)
                throw new ReportesExcepciones("Debe proporcionar una lista de guías.", null);

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var hoja = workbook.Worksheets.Add("Guias");

                    string[] encabezados = { "Título", "Problema", "Solución" };
                    for (int i = 0; i < encabezados.Length; i++)
                        hoja.Cell(1, i + 1).Value = encabezados[i];

                    hoja.Row(1).Style.Font.Bold = true;
                    hoja.Row(1).Style.Font.FontColor = XLColor.White;
                    hoja.Row(1).Style.Fill.BackgroundColor = XLColor.FromArgb(21, 50, 80);

                    int fila = 2;
                    foreach (Guia g in guias)
                    {
                        hoja.Cell(fila, 1).Value = g.Titulo;
                        hoja.Cell(fila, 2).Value = g.Problema;
                        hoja.Cell(fila, 3).Value = g.Solucion;
                        fila++;
                    }

                    hoja.Columns().AdjustToContents();
                    hoja.SheetView.FreezeRows(1);
                    hoja.RangeUsed().SetAutoFilter();

                    using (var ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ReportesExcepciones("Error al generar el Excel de guías", ex);
            }
        }

        // ---------- Usuarios ----------

        public static byte[] GenerarPdfListadoUsuarios(List<Usuario> usuarios, string tituloReporte = "Listado de Usuarios")
        {
            if (usuarios == null)
                throw new ReportesExcepciones("Debe proporcionar una lista de usuarios.", null);

            try
            {
                var documento = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4.Landscape());
                        page.Margin(0);
                        page.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(9));

                        Encabezado(page, tituloReporte, usuarios.Count);

                        page.Content().Padding(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                void CeldaEncabezado(string texto)
                                {
                                    header.Cell().Element(c => c.Background(ColorAcero).Padding(5))
                                        .Text(texto).FontColor(Colors.White).Bold();
                                }

                                CeldaEncabezado("Nombre");
                                CeldaEncabezado("Apellido");
                                CeldaEncabezado("Usuario");
                                CeldaEncabezado("Correo");
                                CeldaEncabezado("Rol");
                                CeldaEncabezado("Activo");
                            });

                            int fila = 0;
                            foreach (Usuario u in usuarios)
                            {
                                Color fondo = fila % 2 == 0 ? Colors.White : ColorFilaPar;

                                table.Cell().Background(fondo).Padding(4).Text(u.Nombre);
                                table.Cell().Background(fondo).Padding(4).Text(u.Apellido);
                                table.Cell().Background(fondo).Padding(4).Text(u.UsuarioLogin);
                                table.Cell().Background(fondo).Padding(4).Text(u.Correo);
                                table.Cell().Background(fondo).Padding(4).Text(u.Rol);
                                table.Cell().Background(fondo).Padding(4).Text(u.Estado ? "Sí" : "No");

                                fila++;
                            }
                        });

                        PieDePagina(page);
                    });
                });

                return documento.GeneratePdf();
            }
            catch (Exception ex)
            {
                throw new ReportesExcepciones("Error al generar el PDF de usuarios", ex);
            }
        }

        public static byte[] GenerarExcelListadoUsuarios(List<Usuario> usuarios)
        {
            if (usuarios == null)
                throw new ReportesExcepciones("Debe proporcionar una lista de usuarios.", null);

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var hoja = workbook.Worksheets.Add("Usuarios");

                    string[] encabezados = { "Nombre", "Apellido", "Usuario", "Correo", "Rol", "Activo" };
                    for (int i = 0; i < encabezados.Length; i++)
                        hoja.Cell(1, i + 1).Value = encabezados[i];

                    hoja.Row(1).Style.Font.Bold = true;
                    hoja.Row(1).Style.Font.FontColor = XLColor.White;
                    hoja.Row(1).Style.Fill.BackgroundColor = XLColor.FromArgb(21, 50, 80);

                    int fila = 2;
                    foreach (Usuario u in usuarios)
                    {
                        hoja.Cell(fila, 1).Value = u.Nombre;
                        hoja.Cell(fila, 2).Value = u.Apellido;
                        hoja.Cell(fila, 3).Value = u.UsuarioLogin;
                        hoja.Cell(fila, 4).Value = u.Correo;
                        hoja.Cell(fila, 5).Value = u.Rol;
                        hoja.Cell(fila, 6).Value = u.Estado ? "Sí" : "No";
                        fila++;
                    }

                    hoja.Columns().AdjustToContents();
                    hoja.SheetView.FreezeRows(1);
                    hoja.RangeUsed().SetAutoFilter();

                    using (var ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ReportesExcepciones("Error al generar el Excel de usuarios", ex);
            }
        }

        // ---------- Áreas ----------

        public static byte[] GenerarPdfListadoAreas(List<Area> areas, string tituloReporte = "Listado de Áreas")
        {
            if (areas == null)
                throw new ReportesExcepciones("Debe proporcionar una lista de áreas.", null);

            try
            {
                var documento = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(0);
                        page.DefaultTextStyle(x => x.FontFamily("Segoe UI").FontSize(10));

                        Encabezado(page, tituloReporte, areas.Count);

                        page.Content().Padding(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns => columns.RelativeColumn());

                            table.Header(header =>
                            {
                                header.Cell().Element(c => c.Background(ColorAcero).Padding(5))
                                    .Text("Nombre del Área").FontColor(Colors.White).Bold();
                            });

                            int fila = 0;
                            foreach (Area a in areas)
                            {
                                Color fondo = fila % 2 == 0 ? Colors.White : ColorFilaPar;
                                table.Cell().Background(fondo).Padding(5).Text(a.NombreArea);
                                fila++;
                            }
                        });

                        PieDePagina(page);
                    });
                });

                return documento.GeneratePdf();
            }
            catch (Exception ex)
            {
                throw new ReportesExcepciones("Error al generar el PDF de áreas", ex);
            }
        }

        public static byte[] GenerarExcelListadoAreas(List<Area> areas)
        {
            if (areas == null)
                throw new ReportesExcepciones("Debe proporcionar una lista de áreas.", null);

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var hoja = workbook.Worksheets.Add("Areas");

                    hoja.Cell(1, 1).Value = "Nombre del Área";
                    hoja.Row(1).Style.Font.Bold = true;
                    hoja.Row(1).Style.Font.FontColor = XLColor.White;
                    hoja.Row(1).Style.Fill.BackgroundColor = XLColor.FromArgb(21, 50, 80);

                    int fila = 2;
                    foreach (Area a in areas)
                    {
                        hoja.Cell(fila, 1).Value = a.NombreArea;
                        fila++;
                    }

                    hoja.Columns().AdjustToContents();
                    hoja.SheetView.FreezeRows(1);
                    hoja.RangeUsed().SetAutoFilter();

                    using (var ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ReportesExcepciones("Error al generar el Excel de áreas", ex);
            }
        }
    }
}