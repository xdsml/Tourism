namespace menu;

using System.Collections.ObjectModel;

public partial class BrimoSnack : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public BrimoSnack()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "brimo1.jpg",
            "brimo.jpg",
            "brimo2.jpg",
            "brimo3.jpg",
            "brimo4.jpg"

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
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=Brimo+Snack+Oujda";
        await Launcher.Default.OpenAsync(locationUrl);
    }



}



