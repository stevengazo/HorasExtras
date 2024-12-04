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
    public Command IDeleteExtra { get; set; }
    public Command<Extras> IEditExtra { get; private set; }

    public ViewProjectVM()
    {
        ProjectObj = SharedData.SelectedProject;
        IAddExtra = new Command(() => AddExtra());
        ILoadExtras = new Command(() => LoadExtras());
        IDeleteProject = new Command(() => DeleteProject());
        IDeleteExtra = new Command<Extras>(async (parameter) => await DeleteExtra(parameter));
        IEditExtra = new Command<Extras>(async (parameter) => await EditExtra(parameter));
        //   LoadExtras();
    }

    private async Task EditExtra(Extras parameter)
    {
        try
        {
            SharedData.SelectedExtra = parameter;
            await Shell.Current.GoToAsync(nameof(EditExtra));
        }
        catch (System.Exception E)
        {
            Console.WriteLine(E.Message);
        }
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

    private async Task DeleteExtra(Extras e)
    {
        try
        {
            bool response = await Application.Current.MainPage.DisplayAlert(
               "Pregunta",    // Título del diálogo
               "Desea Borrar esta extra",  // Mensaje del diálogo
               "Sí",     // Texto del botón Sí
               "No"      // Texto del botón No
           );

            if (response)
            {
                using (var db = new ProjectHoursContext())
                {
                    db.Extras.Remove(e);
                    db.SaveChanges();
                }
                this.Extras.Remove(e);
            }
            // await Shell.Current.GoToAsync("ViewExtra");

        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

}
