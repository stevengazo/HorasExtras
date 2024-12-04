using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using CommunityToolkit.Mvvm.Input;
using HorasExtras.Data;
using HorasExtras.Models;
using Microsoft.EntityFrameworkCore;

namespace HorasExtras.ViewModels;

public class ViewProjectVM : INotifyPropertyChangedAbs
{
    private ObservableCollection<Extras> _extras { get; set; } = new();
    private Project _Project { get; set; } = new();
    public ObservableCollection<Extras> Extras
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
                OnPropertyChanged(nameof(Extras));
            }
        }
    }
    public Project ProjectObj
    {
        get { return _Project; }
        set
        {
            _Project = value;
            if (_Project != null)
            {
                OnPropertyChanged(nameof(ProjectObj));
            }
        }
    }
    public Command IAddExtra
    {
        get;
        set;
    }
    public Command IDeleteProject { get; set; }
    public Command ILoadExtras { get; set; }
    public Command IViewExtras { get; set; }
    public ViewProjectVM()
    {
        ProjectObj = SharedData.SelectedProject;
        IAddExtra = new Command(() => AddExtra());
        ILoadExtras = new Command(() => LoadExtras());
        IDeleteProject = new Command(() => DeleteProject());
        IViewExtras = new Command(() => ViewExtras());
        //   LoadExtras();
    }

    private async Task DeleteProject()
    {
        try
        {
            // Mostrar el cuadro de diálogo con opciones Sí y No
            bool response = await Application.Current.MainPage.DisplayAlert(
                "Pregunta",    // Título del diálogo
                "Deseas borrar este proyecto y sus extras",  // Mensaje del diálogo
                "Sí",     // Texto del botón Sí
                "No"      // Texto del botón No
            );
            if (response)
            {
                using (var db = new ProjectHoursContext())
                {
                    var extras = db.Extras.Where(x => x.ProjectId == SharedData.SelectedProject.ProjectId);
                    db.RemoveRange(extras);
                    db.SaveChanges();
                    db.Projects.Remove(ProjectObj);
                    db.SaveChanges();
                    await ClosePage();
                }
            }
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    private async Task AddExtra()
    {
        try
        {
            await Shell.Current.GoToAsync("AddExtra");
        }
        catch (System.Exception er)
        {

            throw;
        }

    }
    private async Task DeleteExtra()
    {
        try
        {

        }
        catch (System.Exception ex)
        {

            throw;
        }
    }
    private async Task ClosePage()
    {
        await Shell.Current.GoToAsync("..");
    }
    private async Task LoadExtras()
    {
        try
        {
            this.Extras.Clear();
            using (var db = new ProjectHoursContext())
            {
                var Ext = db.Extras.Include(e => e.Employee).Where(e => e.ProjectId == SharedData.SelectedProject.ProjectId).ToList();
                foreach (var Extra in Ext)
                {
                    this.Extras.Add(Extra);
                }
            }
        }
        catch (System.Exception ex)
        {

            throw;
        }
    }

    private void ViewExtras()
    {
        try
        {
             bool response = Application.Current.MainPage.DisplayAlert(
                "Pregunta",    // Título del diálogo
                "¿Deseas borrar esta hora extra?",  // Mensaje del diálogo
                "Sí",     // Texto del botón Sí
                "No"      // Texto del botón No
            ).Result;
               Shell.Current.GoToAsync("ViewExtra");

        }catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);  
        }
    }

}
