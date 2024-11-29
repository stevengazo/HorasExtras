using System;

namespace HorasExtras.ViewModels;
using System;
using System.Threading.Tasks;
using HorasExtras.Data;
using HorasExtras.Models;

public class AddExtraVM : INotifyPropertyChangedAbs
{
    public string[] employees { get; set; }

    public AddExtraVM()
    {
        // Inicializamos el comando para que cierre la página
        Close = new Command(async () => await ClosePage());
        LoadEmployees();
    }

    private Extras _Extra;
    public Extras Extra
    {
        get { return _Extra; }
        set { _Extra = value; }
    }

    public Command IAddExtra { get; set; }
    public Command Close { get; }


    private async Task AddExtra()
    {
        try
        {
            using (var db = new ProjectHoursContext()){
                var employeeName =  
            }


        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    private async Task ClosePage()
    {
        await Shell.Current.GoToAsync("..");
    }
    private async Task LoadEmployees()
    {
        using (var db = new ProjectHoursContext())
        {
            employees = (from emp in db.Employees
                         select $"{emp.EmployeeName} {emp.LastName} {emp.SecondLastName}").ToArray();

        }
    }


}
