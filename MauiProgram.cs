
using course4Hotel.Models;
using course4Hotel.ViewModels;
using course4Hotel.View;
using Microsoft.Extensions.Logging;
using Firebase.Database;
using course4Hotel.DataServices;

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
     
            builder.Services.AddSingleton<RoomsViewModel>();
            builder.Services.AddSingleton<AdminRooms>();
            builder.Services.AddSingleton<NotificationsAdmin>();
            builder.Services.AddSingleton<OfServicesViewModel>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<SinginPage>();
            builder.Services.AddSingleton<HeaderView>();
            builder.Services.AddSingleton<AdminServices>();
            builder.Services.AddSingleton(new FirebaseClient("https://course4hotel-default-rtdb.firebaseio.com/"));
            return builder.Build();

        }
    }
}
