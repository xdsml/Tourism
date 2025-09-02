namespace menu;

using System.Collections.ObjectModel;

public partial class citerne : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public citerne()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "citerne.jpg",
            "citerne1.jpg",
            "citerne2.jpg",
            "citerne3.jpg",
            "citerne4.jpg",
            "citerne5.jpg"
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
    private async void OpenGoogleMaps(object sender, EventArgs e)
    {
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=Citerne+Portugaise+El+Jadida";
        await Launcher.Default.OpenAsync(locationUrl);
    }
    private async void OnCompassClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
    }

}



