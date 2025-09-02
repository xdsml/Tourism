namespace menu;

using System.Collections.ObjectModel;

public partial class Kasbah : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public Kasbah()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "kasbah.jpg",
            "Kasbah1.jpg",
            "Kasbah2.jpg",
            "Kasbah3.jpg",
            "Kasbah4.jpg",
            "Kasbah5.jpg"
        };

        // Associer les images au BindingContext
        BindingContext = this;
    }
    private async void home(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new MainPage());
    }

    private async void profil(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new profil());
    }
    private async void OnCompassClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
    }
    private async void OpenGoogleMaps(object sender, EventArgs e)
    {
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=Kasbah+de+Taourirt+Ouarzazate";
        await Launcher.Default.OpenAsync(locationUrl);
    }

}



