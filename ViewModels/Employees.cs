using System;
using System.Collections.ObjectModel;
using HorasExtras.Data;
using HorasExtras.Models;
using SQLitePCL.lib;

namespace HorasExtras.ViewModels;

public class Employees : INotifyPropertyChangedAbs
{
    public Employees()
    {

    }



    private ObservableCollection<Employee> _EmployeeList { get; set; } = new();

    public ObservableCollection<Employee> EmployeeList
    {
        get { return _EmployeeList; }
        set
        {
            _EmployeeList = value;
            if (_EmployeeList != null)
            {
                OnPropertyChanged(nameof(EmployeeList));
            }
        }
    }

    public Command IAddEmployee
    {
        get
        {
            return new Command(async () => await AddEmployeeAsync());
        }
        private set
        {

        }
    }
    private async Task AddEmployeeAsync()
    {
        try
        {
            var EmployeeName = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Empleado", "Indique el nombre", "OK", "Cancelar");
            var EmployeeLastname1 = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Empleado", "Indique el Primer Apellido", "OK", "Cancelar");
            var EmployeeLastname2 = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Empleado", "Indique el Segundo Apellido", "OK", "Cancelar");

            Employee newEmployee = new Employee()
            {
                EmployeeId = Guid.NewGuid().ToString(),
                EmployeeName = EmployeeName,
                LastName = EmployeeLastname1,
                SecondLastName = EmployeeLastname2
            };
            EmployeeList.Add(newEmployee);

            using (var db = new  ProjectHoursContext()){
                db.Employees.Add(newEmployee);
                db.SaveChanges();
            }
            OrderList();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
        }
    }

    private void OrderList()
    {
        var list = EmployeeList.ToList();
        list.Sort((e1, e2) => e1.EmployeeName.CompareTo(e2.EmployeeName));
        EmployeeList.Clear();
        foreach (var item in list)
        {
            EmployeeList.Add(item);
        }
    }
}
