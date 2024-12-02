using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using HorasExtras.Data;
using HorasExtras.Models;
using HorasExtras.Views;

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
  public Command ILoadProjects { get; set; }
    public IAsyncRelayCommand<Project> IViewProject
    {
        get;
        set;
    }
    public MainPageVM()
    {
      /// LoadProjectsAsync();
        IViewProject = new AsyncRelayCommand<Project>(ViewProject);
        ILoadProjects = new Command( async () => await LoadProjectsAsync());
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
                OrderProjectsAsync();
                using (var db = new ProjectHoursContext())
                {
                    db.Projects.Add(newProject);
                    db.SaveChanges();
                }

            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    private async Task LoadProjectsAsync()
    {
        Projects.Clear();
        using (var db = new ProjectHoursContext())
        {
            var projects = db.Projects.OrderBy(e => e.ProjectName).ToList();
            foreach (var project in projects)
            {
                Projects.Add(project);
            }
        }
    }
    private async Task OrderProjectsAsync()
    {
        var pList = Projects.OrderBy(e => e.ProjectName).ToList();
        Projects.Clear();
        foreach (var project in pList)
        {
            Projects.Add(project);
        }
    }

    private async Task ViewProject(Project project)
    {
        try
        {
            SharedData.SelectedProject = project;
            await Shell.Current.GoToAsync("ViewProject");
        }
        catch (System.Exception er)
        {

            throw;
        }

    }
}
