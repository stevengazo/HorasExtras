using System;

namespace HorasExtras.ViewModels;
using System;
using System.Threading.Tasks;
using HorasExtras.Data;
using HorasExtras.Models;
using HorasExtras.Data;

public class AddExtraVM : INotifyPropertyChangedAbs
{
    public string[] employees { get; set; }

    public AddExtraVM()
    {
        // Inicializamos el comando para que cierre la página
        Close = new Command(async () => await ClosePage());
        IAddExtra = new Command(async () => await AddExtra());
        LoadEmployees();
    }
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
    private Extras _Extra { get; set; } = new Extras()
    {
        Day = DateTime.Today,
        EntryHour = DateTime.Now.TimeOfDay,
        ExitHour = DateTime.Now.TimeOfDay
    };
    public Extras Extra
    {
        get { return _Extra; }
        set
        {
            _Extra = value;
            if (_Extra != null)
            {
                OnPropertyChanged(nameof(Extra));
            }
        }
    }

    public Command IAddExtra { get; set; }
    public Command Close { get; }
    private async Task AddExtra()
    {

        try
        {
            if (EmployeeName != null)
            {
                using (var db = new ProjectHoursContext())
                {
                    Extra.ProjectId = SharedData.SelectedProject.ProjectId;
                    var employeeFullName = await GetName(EmployeeName);
                    Extra.TotalDuration = Extra.ExitHour - Extra.EntryHour;
                    var employee = db.Employees
                                        .Where(ep => ep.EmployeeName == employeeFullName[0] && ep.LastName == employeeFullName[1])
                                        .FirstOrDefault();
                    Extra.EmployeeId = employee != null ? employee.EmployeeId : 0;
                    db.Extras.Add(Extra);
                    db.SaveChanges();
                    var answer = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Deseas agregar otro registro con la misma información", "Sí", "No");
                    if (!answer)
                    {
                        ClosePage();
                    }
                    else
                    {
                        var tmpExtra = new Extras(){
                            EntryHour = Extra.EntryHour,
                            ExitHour = Extra.ExitHour,
                            Day = Extra.Day,
                            TotalDuration = Extra.TotalDuration,
                            ProjectId = Extra.ProjectId,
                            Notes = Extra.Notes,
                        };
                        Extra = tmpExtra;
                        EmployeeName = "";
                        Application.Current.MainPage.DisplayAlert("información", "Seleccione el siguiente empleado", "OK");
                    }
                }
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

}
