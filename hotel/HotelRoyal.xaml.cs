namespace menu;

using System.Collections.ObjectModel;

public partial class HotelRoyal : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public HotelRoyal()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "royal.jpg",
            "royal1.jpg",
            "royal2.jpg",
            "royal3.jpg",
            "royal4.jpg",
            "royal5.jpg"
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
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=H%C3%B4tel+Royal+Oujda";
        await Launcher.Default.OpenAsync(locationUrl);
    }

}



