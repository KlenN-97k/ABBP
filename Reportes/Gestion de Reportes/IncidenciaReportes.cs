using ClosedXML.Excel;
using Entidades.Gestion_de_Entidades;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Reportes.Gestion_de_Reportes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reportes
{
    public static class IncidenciaReportes
    {
        static IncidenciaReportes()
        {
            // QuestPDF exige declarar el tipo de licencia antes de generar cualquier documento.
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static byte[] GenerarPdfListado(List<Incidencia> incidencias, string tituloReporte = "Reporte de Incidencias")
        {
            try
            {
                var documento = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4.Landscape());
                        page.Margin(30);
                        page.DefaultTextStyle(x => x.FontSize(9));

                        page.Header().Column(col =>
                        {
                            col.Item().Text(tituloReporte).FontSize(16).Bold();
                            col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}  |  Total: {incidencias.Count}")
                                .FontSize(8).FontColor(Colors.Grey.Medium);
                        });

                        page.Content().PaddingTop(10).Table(table =>
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
                                    header.Cell().Element(c => c
                                        .Background(Colors.Grey.Lighten2)
                                        .Padding(4))
                                        .Text(texto).Bold();
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

                            foreach (Incidencia inc in incidencias)
                            {
                                table.Cell().Padding(3).Text(inc.NumeroTicket);
                                table.Cell().Padding(3).Text(inc.Fecha.ToString("dd/MM/yyyy"));
                                table.Cell().Padding(3).Text(inc.Empleado);
                                table.Cell().Padding(3).Text(inc.NombreArea ?? "-");
                                table.Cell().Padding(3).Text(inc.TipoIncidencia);
                                table.Cell().Padding(3).Text(inc.NombrePrioridad ?? "-");
                                table.Cell().Padding(3).Text(inc.NombreEstado ?? "-");
                                table.Cell().Padding(3).Text(inc.TecnicoAsignado ?? "Sin asignar");
                            }
                        });

                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("Página ");
                            x.CurrentPageNumber();
                            x.Span(" de ");
                            x.TotalPages();
                        });
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
                    {
                        hoja.Cell(1, i + 1).Value = encabezados[i];
                    }
                    hoja.Row(1).Style.Font.Bold = true;
                    hoja.Row(1).Style.Fill.BackgroundColor = XLColor.LightGray;

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


        public static byte[] GenerarPdfListadoGuias(List<Guia> guias, string tituloReporte = "Catálogo de Guías")
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
                        page.Margin(30);
                        page.DefaultTextStyle(x => x.FontSize(10));

                        page.Header().Column(col =>
                        {
                            col.Item().Text(tituloReporte).FontSize(16).Bold();
                            col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}  |  Total: {guias.Count}")
                                .FontSize(8).FontColor(Colors.Grey.Medium);
                        });

                        page.Content().PaddingTop(10).Column(col =>
                        {
                            foreach (Guia g in guias)
                            {
                                col.Item().PaddingBottom(10).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Column(item =>
                                {
                                    item.Item().Text(g.Titulo).FontSize(12).Bold();
                                    item.Item().PaddingTop(4).Text(t => { t.Span("Problema: ").Bold(); t.Span(g.Problema); });
                                    item.Item().PaddingTop(2).Text(t => { t.Span("Solución: ").Bold(); t.Span(g.Solucion); });
                                });
                            }
                        });

                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("Página "); x.CurrentPageNumber(); x.Span(" de "); x.TotalPages();
                        });
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
                    hoja.Row(1).Style.Fill.BackgroundColor = XLColor.LightGray;

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
    }
}