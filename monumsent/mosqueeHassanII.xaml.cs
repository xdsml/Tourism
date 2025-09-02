namespace menu;

using System.Collections.ObjectModel;

public partial class mosqueeHassanII : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public mosqueeHassanII()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "hassan.jpeg",
            "mosqueeh1.jpg",
            "mosqueeh2.jpg",
            "mosqueeh3.jpg",
            "mosqueeh4.jpg",
            "mosqueeh5.jpg"
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
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=Mosqu%C3%A9e+Hassan+II+Casablanca";
        await Launcher.Default.OpenAsync(locationUrl);
    }

}



