using System;
using HorasExtras.Data;
using Microsoft.EntityFrameworkCore;

namespace HorasExtras.ViewModels;

public class ConfigurationVM : INotifyPropertyChangedAbs
{
    private string _UserName;
    public string UserName
    {
        get { return _UserName; }
        set
        {
            _UserName = value;
            if (_UserName != null)
            {
                OnPropertyChanged(nameof(UserName));
            }
        }
    }
    public Command IEraseDB { get; set; }
    public Command ISetUser { get; set; }
    public ConfigurationVM()
    {
        IEraseDB = new Command(async () => await DeleteDbAsync());
        ISetUser = new Command(async () => await SetUserProfileAsync());
    }
    private async Task DeleteDbAsync()
    {
        try
        {
            // Mostrar el cuadro de diálogo con opciones Sí y No
            bool responseA = await Application.Current.MainPage.DisplayAlert(
                "Pregunta",    // Título del diálogo
                "Deseas las horas extras y proyectos guardados",  // Mensaje del diálogo
                "Sí",     // Texto del botón Sí
                "No"      // Texto del botón No
            );
            if (responseA)
            {
                using (var db = new ProjectHoursContext())
                {
                    var extras = db.Extras.AsNoTracking().ToList();
                    db.Extras.RemoveRange(extras);
                    var proj = db.Projects.AsNoTracking().ToList();
                    db.Projects.RemoveRange(proj);
                    db.SaveChanges();
                    Application.Current.MainPage.DisplayAlert("Información", "La base de datos fue borrada", "OK");
                }
            }
        }
        catch (System.Exception ex)
        {

            throw;
        }
    }
    private async Task SetUserProfileAsync()
    {
        try
        {
            UserName = string.IsNullOrEmpty(UserName) ? "" : UserName; 
            Preferences.Set(nameof(UserName), UserName);
            Application.Current.MainPage.DisplayAlert("Información", "Usuario Actualizado", "Ok");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
