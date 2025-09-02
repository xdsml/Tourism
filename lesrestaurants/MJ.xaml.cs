namespace menu;

using System.Collections.ObjectModel;

public partial class MJ : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public MJ()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "mj.jpg",
            "mj1.jpg",
            "mj2.jpg",
            "mj3.jpg",
            "mj4.jpg",
            "mj5.jpg"
        };

        // Associer les images au BindingContext
        BindingContext = this;
    }

    private async void OnAccueilClicked(object sender, EventArgs e)
    {
        // Navigation vers la page principale (décommenter si nécessaire)
        // await Navigation.PushAsync(new MainPage());
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
    private async void OnCompassClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
    }
    private async void OpenGoogleMaps(object sender, EventArgs e)
    {
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=M%26J+Resto+Oujda";
        await Launcher.Default.OpenAsync(locationUrl);
    }
}
