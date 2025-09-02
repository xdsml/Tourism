namespace menu;

using System.Collections.ObjectModel;

public partial class DrifFood : ContentPage
{
    public ObservableCollection<string> Images { get; set; }

    public DrifFood()
    {
        InitializeComponent();

        // Initialisation des images du slider
        Images = new ObservableCollection<string>
        {
            "driif.jpg",
            "driif1.jpg",
            "driif2",
            "driif3.jpg",
            "driif4.jpg",
            "driif5.jpg"
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





}



