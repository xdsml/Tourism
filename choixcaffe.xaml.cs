namespace menu;

public partial class choixcaffe : ContentPage
{
    public choixcaffe()
    {
        InitializeComponent();
    }
    private async void bleu(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new BleuCafé());
    }
    private async void excell(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new LExcellence());
    }
    private async void profil(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new profil());
    }

    private async void home(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new MainPage());
    }

    private async void eme(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new lEmerdraude());
    }

    private async void amy(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new amy());
    }
    private async void OnCompassClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
    }

}