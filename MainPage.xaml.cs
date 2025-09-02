using menu.matches;



namespace menu
{
    public partial class MainPage : ContentPage
    {

        // Liste des suggestions
      

        public MainPage()
        {
            InitializeComponent();
        }

        private async void restau(object sender, EventArgs e)
        {
            // Utilise await pour une navigation asynchrone propre
            await Navigation.PushAsync(new restaurants { Title = "" });
        }

        
        private async void profil(object sender, EventArgs e)
        {
            // Naviguer vers la nouvelle page
            await Navigation.PushAsync(new profil());
        }
        private async void hotel(object sender, EventArgs e)
        {
            // Naviguer vers la nouvelle page
            await Navigation.PushAsync(new choixhotel());
        }
        private async void match(object sender, EventArgs e)
        {
            // Naviguer vers la nouvelle page
            await Navigation.PushAsync(new matchelive());
        }

        private async void monu(object sender, EventArgs e)
        {
            // Naviguer vers la nouvelle page
            await Navigation.PushAsync(new choixmonument());
        }
        private async void OnScanTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new menu.Qr.Qrscore());
        }
        private async void OnCloseScannerClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }


        private async void OnCompassClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
        }
        private async void Onsupporttapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Contact()); // Ouvre ta page pour publier
        }
        

        // Gère le changement de texte dans la barre de recherche

        // Gère le clic sur une suggestion




    }

}
