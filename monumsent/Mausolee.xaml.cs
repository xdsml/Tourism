namespace menu;

using System.Collections.ObjectModel;

public partial class Mausolee : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public Mausolee()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "mausolee.jpg",
            "mausolee1.jpg",
            "mausolee2.jpg",
            "mausolee3.jpg",
            "mausolee4.jpg",
            "mausolee5.jpg"
        };

        // Associer les images au BindingContext
        BindingContext = this;
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
    private async void profil(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new profil());
    }

    private async void OpenGoogleMaps(object sender, EventArgs e)
    {
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=Mausol%C3%A9e+Mohammed+V+Rabat";
        await Launcher.Default.OpenAsync(locationUrl);
    }

}



