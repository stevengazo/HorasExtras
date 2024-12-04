using System;
using HorasExtras.Data;
using HorasExtras.Models;

namespace HorasExtras.ViewModels;

public class EditExtraVM : INotifyPropertyChangedAbs
{
    public string[] employees { get; set; } = new string[] { "" };
    private string _EmployeeName { get; set; } = "";
    public string EmployeeName
    {
        get
        {
            return _EmployeeName;
        }
        set
        {
            _EmployeeName = value;
            if (_EmployeeName != null)
            {
                OnPropertyChanged(nameof(EmployeeName));
            }
        }
    }
    private Extras _extra = new();
    public Extras Extra
    {
        get { return _extra; }
        set
        {
            _extra = value;
            if (_extra != null)
            {
                OnPropertyChanged(nameof(Extra));
            }
        }
    }
    public Command IClosePage { get; private set; }
    public Command IUpdateExtra { get; private set; }


    public EditExtraVM()
    {
        Extra = SharedData.SelectedExtra;
        IClosePage = new Command(async () => await ClosePageAsync());
        IUpdateExtra = new Command(async () => await UpdateExtraAsync());
        LoadEmployees();
        
        if (Extra != null)
        {
            EmployeeName = $"{Extra.Employee.EmployeeName} {Extra.Employee.LastName} {Extra.Employee.SecondLastName}";
        }
    }
    private async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
    private async Task<string[]> GetName(string name)
    {
        return name.Split(' ').ToArray();
    }
    private async Task LoadEmployees()
    {

        using (var db = new ProjectHoursContext())
        {
            employees = (from emp in db.Employees
                         select $"{emp.EmployeeName} {emp.LastName} {emp.SecondLastName}").ToArray();

        }
    }

    private async Task UpdateExtraAsync()
    {
        try
        {
            using (var db = new ProjectHoursContext())
            {
                db.Extras.Update(Extra);
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

}
