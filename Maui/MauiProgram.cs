using Maui.Services.Api;
using Maui.Services.Token;
using Maui.ViewModels;
using Microsoft.Extensions.Logging;

namespace Maui
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

            builder.Services.AddSingleton<ITokenService, TokenService>();
            builder.Services.AddSingleton<IApiService, ApiService>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<Views.LoginPage>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<Views.RegisterPage>();
            builder.Services.AddTransient<MeViewModel>();
            builder.Services.AddTransient<Views.MePage>();

#endif

            return builder.Build();
        }
    }
}
