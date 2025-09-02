namespace menu;

using System.Collections.ObjectModel;

public partial class BigDal : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public BigDal()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
           // "driif.jpg",
            "driif1.jpg",
           // "driif2.jpg",
            "driif3.jpg",
            "bigdal.jpg",
          //  "driif5.jpg"
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
        var locationUrl = "https://www.google.com/maps/search/?api=1&query=Big+Dal+Oujda";
        await Launcher.Default.OpenAsync(locationUrl);
    }



}



