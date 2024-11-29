using System;
using CommunityToolkit.Mvvm.Input;
using HorasExtras.Data;
using HorasExtras.Models;

namespace HorasExtras.ViewModels;

public class ViewProjectVM : INotifyPropertyChangedAbs
{
  private Project _Project { get; set; } = new();
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
        IAddExtra = new Command( ()=> AddExtra());
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
}
