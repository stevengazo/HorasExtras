using System;
using System.Collections.ObjectModel;
using HorasExtras.Data;
using HorasExtras.Models;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace HorasExtras.ViewModels;

public class Export : INotifyPropertyChangedAbs
{
    private DateTime _InitialDate { get; set; } = DateTime.Today;
    public DateTime InitialDate
    {
        get
        {
            return _InitialDate;
        }
        set
        {
            _InitialDate = value;
            if (_InitialDate != null)
            {
                OnPropertyChanged(nameof(InitialDate));
            }

        }
    }
    private DateTime _FinalDate { get; set; } = DateTime.Today;
    public DateTime FinalDate
    {
        get
        {
            return _FinalDate;
        }
        set
        {
            _FinalDate = value;
            if (_FinalDate != null)
            {
                OnPropertyChanged(nameof(FinalDate));
            }

        }
    }
    private ObservableCollection<Extras> _extras { get; set; } = new();
    public ObservableCollection<Extras> ListExtras
    {
        get
        {
            return _extras;
        }
        set
        {
            _extras = value;
            if (_extras != null)
            {
                OnPropertyChanged(nameof(ListExtras));
            }
        }
    }

    public Export()
    {
        ISearch = new Command(async () => SearchExtrasAsync());
        IShare = new Command(async () => DownloadReport());
        IClean = new Command(async () => Clean());
    }
    public Command IClean { get; set; }
    public Command IShare { get; set; }
    public Command ISearch { get; set; }
    private async Task SearchExtrasAsync()
    {
        ListExtras.Clear();
        using (var db = new ProjectHoursContext())
        {
            var results = await (from i in db.Extras
                                 where i.Day >= InitialDate && i.Day <= FinalDate
                                 orderby i.Day descending
                                 select i)
                                .Include(e => e.Employee)
                                .Include(e => e.Project)
                                .AsNoTracking()
                                .ToListAsync();
            foreach (var extras in results)
            {
                ListExtras.Add(extras);
            }

            var errorsExtras = (from i in results
            where i.EntryHour> i.ExitHour 
            select i).ToList();
            if(errorsExtras.Count > 0) {
                Application.Current.MainPage.DisplayAlert("Advertencia","Existen registros de horas extras que estan mal","OK");
            }

        }
    }

    private async Task Clean()
    {
        InitialDate = DateTime.Today;
        FinalDate = DateTime.Today;
        ListExtras.Clear();
    }
    private async Task DownloadReport()
    {
        try
        {
            if (ListExtras.Count > 0)
            {
                var BuilderPDF = new Utilities.PDFGenerate();
                var DocPDF = await BuilderPDF.GenerateReport(ListExtras.ToList());
                var filePath = Path.Combine(FileSystem.CacheDirectory, $"Reporte Extras {DateTime.Now:yy-MM-dd hh-mm-ss}.pdf");
                File.WriteAllBytes(filePath, DocPDF);
                await ShareFile(filePath);
            }
        }
        catch (Exception e)
        {

        }
    }
    private async Task ShareFile(string filePath)
    {
        try
        {
            // Compartir el archivo PDF usando el servicio de MAUI
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Compartir Reporte de Entrada y Salida",
                File = new ShareFile(filePath) // Usar "File" en lugar de "Files"
            });
        }
        catch (Exception ex)
        {
            // Manejo de errores en caso de que no se pueda compartir el archivo
            await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo compartir el archivo: {ex.Message}", "OK");
        }
    }
}
