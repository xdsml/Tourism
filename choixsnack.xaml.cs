using Android.OS;

namespace menu;

public partial class choixsnack : ContentPage
{
    public choixsnack()
    {
        InitializeComponent();
    }

    private async void sah(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new sahbi());
    }
    private async void driif(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DrifFood());
    }

    private async void profil(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new profil());
    }

    private async void big(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new BigDal());
    }

    private async void brimo(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new BrimoSnack());
    }
    private async void home(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new MainPage());
    }
}
