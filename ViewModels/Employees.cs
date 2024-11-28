using System;

namespace HorasExtras.ViewModels;

public class Employees : INotifyPropertyChangedAbs
{
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

        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}
