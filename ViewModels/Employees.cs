using System;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using HorasExtras.Data;
using HorasExtras.Models;

namespace HorasExtras.ViewModels;

public class Employees : INotifyPropertyChangedAbs
{
    public Employees()
    {
        IDeleteEmployee = new Command<Employee>(DeleteEmployee);
        IEditEmployee = new AsyncRelayCommand<Employee>(EditEmployee);
        LoadEmployees();
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
    public Command IDeleteEmployee
    {
        get;
        private set;
    }
    public IAsyncRelayCommand<Employee> IEditEmployee
    {
        get;
        set;
    }
    private async Task AddEmployeeAsync()
    {
        try
        {
            var EmployeeName = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Empleado", "Indique el nombre", "OK", "Cancelar", "Primer Nombre", 12, Keyboard.Text);
            var EmployeeLastname1 = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Empleado", "Indique el Primer Apellido", "OK", "Cancelar", "Primer Apellido", 15, Keyboard.Text);
            var EmployeeLastname2 = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Empleado", "Indique el Segundo Apellido", "OK", "Cancelar", "Segundo Apellido", 15, Keyboard.Text);
            if (string.IsNullOrEmpty(EmployeeName) || string.IsNullOrEmpty(EmployeeLastname1) || string.IsNullOrEmpty(EmployeeLastname2))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Los datos no pueden estar vacios", "OK");
            }
            else
            {
                Employee newEmployee = new Employee()
                {
                    EmployeeName = EmployeeName.Trim(),
                    LastName = EmployeeLastname1.Trim(),
                    SecondLastName = EmployeeLastname2.Trim()
                };
                EmployeeList.Add(newEmployee);

                using (var db = new ProjectHoursContext())
                {
                    db.Employees.Add(newEmployee);
                    db.SaveChanges();
                }
                OrderList();
            }


        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
    private void DeleteEmployee(Employee employee)
    {
        using (var db = new ProjectHoursContext())
        {
            var depende = db.Extras.Where(x => x.EmployeeId == employee.EmployeeId).Any();
            if (depende)
            {
                Application.Current.MainPage.DisplayAlert("Error", "El empleado tiene horas registradas", "ok");
            }
            else
            {
                db.Employees.Remove(employee);
                db.SaveChanges();
                EmployeeList.Remove(employee);
                OrderList();
            }
        }
    }
    private async Task EditEmployee(Employee employee)
    {
        try
        {


            var EmployeeName = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Empleado", "Indique el nombre", "OK", "Cancelar");
            var EmployeeLastname1 = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Empleado", "Indique el Primer Apellido", "OK", "Cancelar");
            var EmployeeLastname2 = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Empleado", "Indique el Segundo Apellido", "OK", "Cancelar");

            if (string.IsNullOrEmpty(EmployeeName) || string.IsNullOrEmpty(EmployeeLastname1) || string.IsNullOrEmpty(EmployeeLastname2))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Los datos no pueden estar vacios", "OK");
            }
            else
            {
                EmployeeList.Remove(employee);
                employee.EmployeeName = EmployeeName;
                employee.LastName = EmployeeLastname1;
                employee.SecondLastName = EmployeeLastname2;

                using (var db = new ProjectHoursContext())
                {
                    db.Employees.Update(employee);
                    db.SaveChanges();
                }
                Application.Current.MainPage.DisplayAlert("Atención", "Usuario Actualizado", "Ok");

                EmployeeList.Remove(employee);
                EmployeeList.Add(employee);
                OrderList();
            }
        }
        catch (Exception r)
        {
            Application.Current.MainPage.DisplayAlert("Error", r.Message, "Ok");
            throw;
        }
    }
    private async Task LoadEmployees()
    {
        using (var db = new ProjectHoursContext())
        {
            var emp = db.Employees.OrderBy(e => e.EmployeeName).ToList();
            foreach (var item in emp)
            {
                EmployeeList.Add(item);
            }
        }


    }
    private async Task OrderList()
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
