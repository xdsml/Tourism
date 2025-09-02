namespace menu;

public partial class choixmonument : ContentPage
{
    public choixmonument()
    {
        InitializeComponent();
    }
    private async void Mausole(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new Mausolee());
    }
    private async void home(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new MainPage());
    }
    private async void profil(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new profil());
    }
    private async void citerne(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new citerne());
    }
    private async void Kasbah(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new Kasbah());
    }
    private async void mosque(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new mosqueeHassanII());
    }
    private async void OnCompassClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
    }
}