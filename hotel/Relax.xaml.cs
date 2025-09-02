namespace menu;

using System.Collections.ObjectModel;

public partial class Relax : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public Relax()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "relax.jpg",
            "relax1.jpg",
            "relax2.jpg",
            "relax3.jpg",
            "relax4.jpg",
            "relax5.jpg"
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
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=Relax+Hotel+Oujda";
        await Launcher.Default.OpenAsync(locationUrl);
    }

}



