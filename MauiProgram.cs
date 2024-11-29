using HorasExtras.Data;
using HorasExtras.ViewModels;
using Microsoft.Extensions.Logging;

namespace HorasExtras;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddDbContext<ProjectHoursContext>();
		builder.Services.AddTransient<MainPage>();

		builder.Services.AddTransient<MainPageVM>();
		
		var dbContext = new ProjectHoursContext();

		dbContext.Database.EnsureCreated();
		dbContext.Dispose();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
