namespace menu;

public partial class choixrestau : ContentPage
{
    public choixrestau()
    {
        InitializeComponent();
    }
    private async void belleotient(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new belleorient());
    }
    private async void mj(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new MJ());
    }
    private async void lunaria(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new Lunaria());
    }
    private async void profil(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new profil());
    }

    private async void piri(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new piri());
    }

    private async void home(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new MainPage());
    }
    private async void OnCompassClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
    }
}