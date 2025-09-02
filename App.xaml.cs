using menu.Pages;

namespace menu
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            // Délai de navigation : attendre que Shell soit prêt
            Microsoft.Maui.Controls.Application.Current?.Dispatcher.Dispatch(async () =>
            {
                await Shell.Current.GoToAsync("//SignIn");
            });
        }

    }
}
