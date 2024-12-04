using HorasExtras.Views;

namespace HorasExtras;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("ViewProject", typeof(ViewProject));
		Routing.RegisterRoute("AddExtra", typeof(AddExtra));
		Routing.RegisterRoute("ViewExtra", typeof(ViewExtra));
		Routing.RegisterRoute(nameof(EditExtra), typeof(EditExtra));
	}
}
