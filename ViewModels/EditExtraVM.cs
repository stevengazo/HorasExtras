using System;
using HorasExtras.Data;
using HorasExtras.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

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
        LoadEmployees();

    

        using (var db = new ProjectHoursContext())
        {
            Extra = db.Extras.Include(e=>e.Employee).FirstOrDefault(e => e.ExtraId == SharedData.SelectedExtra.ExtraId);
        }

        IClosePage = new Command(async () => await ClosePageAsync());
        IUpdateExtra = new Command(async () => await UpdateExtraAsync());


        EmployeeName = $"{Extra.Employee.EmployeeName} {Extra.Employee.LastName} {Extra.Employee.SecondLastName}";

    }
    private async Task ClosePageAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
    private async Task<string[]> GetName(string name)
    {
        return name.Split(' ').ToArray();
    }
    private void LoadEmployees()
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
            if (!string.IsNullOrEmpty(EmployeeName))
            {
                var name = await GetName(EmployeeName);

                using (var db = new ProjectHoursContext())
                {
                    Extra.Employee= null;
                    var e = (from i in db.Employees
                             where i.EmployeeName == name[0] && i.LastName == name[1]
                             select i).FirstOrDefault();
                    Extra.EmployeeId = e != null ? e.EmployeeId : throw new NullReferenceException("Error");
                    db.Extras.Update(Extra);
                    db.SaveChanges();
                }
                await ClosePageAsync();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Advertencia", "Verifique el nombre del empleado", "OK");
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

}
