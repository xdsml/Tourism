namespace menu;

using System.Collections.ObjectModel;

public partial class piri : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public piri()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "piri.jpg",
            "piri1.jpg",
            "piri2.jpg",
            "piri3.jpg",
            "piri4.jpg",
            "piri5.jpg"
        };

        // Associer les images au BindingContext
        BindingContext = this;
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
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=Piri+Piri+Poulet+Brais%C3%A9+Oujda";
        await Launcher.Default.OpenAsync(locationUrl);
    }
}
