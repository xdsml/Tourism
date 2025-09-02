using Firebase.Auth.Providers;
using Firebase.Auth;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;
using menu.Pages;

namespace menu;

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
                }).UseBarcodeReader();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyCpibCq4pQ_p5Qwr0kIHC5nBJeV3F61Ruc",
                AuthDomain = "guidsign-c2579.firebaseapp.com",
                Providers = new FirebaseAuthProvider[] {
                    new EmailProvider(),
                    new GoogleProvider()

                }




            }));


        builder.Services.AddSingleton<SignInView>();
        builder.Services.AddSingleton<SignInViewModel>();
        builder.Services.AddSingleton<SignUpView>();
        builder.Services.AddSingleton<SignUpViewModel>();








        return builder.Build();
        }
}
