namespace menu;

using System.Collections.ObjectModel;

public partial class Lunaria : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public Lunaria()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "lunaria.jpg",
            "lunaria1.jpg",
            "lunaria2.jpg",
            "lunaria3.jpg",
            "lunaria4.jpg",
            "lunaria5.jpg"
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
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=Lunaria+Restaurant+Coffee+Club+Oujda";
        await Launcher.Default.OpenAsync(locationUrl);
    }
}
