using System;
using HorasExtras.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace HorasExtras.Utilities;

public class PDFGenerate
{
    public async Task<byte[]> GenerateReport(List<Extras> extras)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);
                Paragraph header = new Paragraph($"Reporte Horas Extras")
                                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                    .SetFontSize(20);
                Paragraph DateInfo = new Paragraph($"Fecha de Generación {DateTime.Now.ToString()}").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(12);
                document.Add(header);
                document.Add(DateInfo);
                addDataTable(document, extras);
                document.Close();
                return ms.ToArray();
            }
            return null;
        }
        catch (System.Exception e)
        {

            throw;
        }
    }
    private void addDataTable(Document document, List<Extras> extrasList)
    {
        // Crear una tabla con 7 columnas
        Table table = new Table(UnitValue.CreatePercentArray(7)).UseAllAvailableWidth();

        // Agregar encabezados
        table.AddHeaderCell("Persona");
        table.AddHeaderCell("Dia");
        table.AddHeaderCell("Proyecto");
        table.AddHeaderCell("Entrada");
        table.AddHeaderCell("Salida");
        table.AddHeaderCell("Total");
        table.AddHeaderCell("Motivo");

        for (int i = 0; i < extrasList.Count; i++)
        {
            AddRow(table, $"{extrasList[i].Employee.EmployeeName} {extrasList[i].Employee.LastName}", extrasList[i].Project.ProjectName, extrasList[i].Day, extrasList[i].EntryHour, extrasList[i].ExitHour, extrasList[i].TotalDuration, extrasList[i].Notes);
        }
        document.Add(table);
    }
    private void AddRow(Table table, string Employee, string ProjectName, DateTime Day, TimeSpan Entry, TimeSpan Exit, TimeSpan TimeSpanTotal, string note)
    {
        table.AddCell(Employee);
        table.AddCell(Day.ToString("dd-MMM-yy"));
        table.AddCell(ProjectName);
        table.AddCell(Entry.ToString(@"hh\:mm"));
        table.AddCell(Exit.ToString(@"hh\:mm"));
        table.AddCell(TimeSpanTotal.ToString(@"hh\:mm"));
        table.AddCell(note);
    }
}
