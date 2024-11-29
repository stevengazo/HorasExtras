using System;
using System.Collections.ObjectModel;
using HorasExtras.Models;

namespace HorasExtras.ViewModels;

public class MainPageVM : INotifyPropertyChangedAbs
{
    private ObservableCollection<Project> _projects = new();
    public ObservableCollection<Project> Projects
    {
        get { return _projects; }
        set
        {
            _projects = value;
            if (_projects != value)
            {
                OnPropertyChanged(nameof(Projects));
            }
        }
    }
    public Command IAddProject
    {
        get
        {
            return new Command(async () => await AddProjectAsync());
        }
        private set { }
    }

    public MainPageVM()
    {

    }


    private async Task AddProjectAsync()
    {
        try
        {
            string Name = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Proyecto", "Indique el nombre", "OK", "Cancelar");
            var typeOptions = new string[] { "Instalación", "Mantenimiento", "Otro" };
            string Type = await Application.Current.MainPage.DisplayActionSheet("Selecciona una opción", "Cancelar", null, typeOptions);
            if (!string.IsNullOrEmpty(Name))
            {
                Project newProject = new Project(Name, Type);

                Projects.Add(newProject);
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}
