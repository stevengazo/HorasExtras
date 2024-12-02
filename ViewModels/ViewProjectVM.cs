using System;
using System.Collections.ObjectModel;
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
    public ViewProjectVM()
    {
        ProjectObj = SharedData.SelectedProject;
        IAddExtra = new Command(() => AddExtra());
        LoadExtras();
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
    private async Task LoadExtras()
    {
        try
        {
            using(var db = new  ProjectHoursContext())
            {
                var Ext = db.Extras.Include(e=>e.Employee).Where(e=> e.ProjectId == SharedData.SelectedProject.ProjectId).ToList();
                foreach(var Extra in Ext){
                    this.Extras.Add(Extra);
                }
            }
        }
        catch (System.Exception ex)
        {

            throw;
        }
    }

}
