using course4Hotel.Data;
using course4Hotel.Models;
using course4Hotel.ViewModels;
using course4Hotel.View;
using Microsoft.Extensions.Logging;

namespace course4Hotel
{
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<RoomsViewModel>();
            builder.Services.AddSingleton<AdminRooms>();
            builder.Services.AddSingleton<OfServicesViewModel>();
            builder.Services.AddSingleton<AdminServices>();
            return builder.Build();
        }
    }
}
