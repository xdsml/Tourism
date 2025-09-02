namespace menu;

using System.Collections.ObjectModel;

public partial class amy : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public amy()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "amy.jpg",
            "amy1.jpg",
            "amy2.jpg",
            "amy3.jpg",
            "amy4.jpg",
            "amy5.jpg"
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
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=Amy's+Garden+Oujda";
        await Launcher.Default.OpenAsync(locationUrl);
    }
    private async void OnCompassClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
    }



}
