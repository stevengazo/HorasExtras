using HorasExtras.Views;

namespace HorasExtras;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("ViewProject",  typeof(ViewProject));
      
	}
}
