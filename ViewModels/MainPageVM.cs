using System;

namespace HorasExtras.ViewModels;

public class MainPageVM : INotifyPropertyChangedAbs
{

    public Command IAddProject
    {
        get
        {
            return new Command( async()=> await AddProject()  );
        }
        private set { }
    }

    public MainPageVM()
    {

    }


    private async Task AddProject()
    {
        try
        {
            var ProjectName = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Proyecto", "Indique el nombre del proyecto", "OK", "Cancelar");
            var ProyectType = await Application.Current.MainPage.DisplayPromptAsync("Nuevo Proyecto", "Indique el tipo de proyecto", "OK", "Cancelar");

        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}
