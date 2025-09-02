namespace menu;

using System.Collections.ObjectModel;

public partial class LExcellence : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public LExcellence()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "excellence.jpg",
            "excellence1.jpg",
            "excellence2.jpg",
            "excellence3.jpg",
            "excellence4.jpg",
            "excellence5.jpg"
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
    private async void OpenGoogleMaps(object sender, EventArgs e)
    {
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=Excellence+Gold+Oujda";
        await Launcher.Default.OpenAsync(locationUrl);
    }
    private async void OnCompassClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
    }


}
