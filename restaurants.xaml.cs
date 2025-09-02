using Microsoft.Maui.Controls;

namespace menu
{
    public partial class restaurants : ContentPage
    {
        public restaurants()
        {
            InitializeComponent();
        }


        private async void choixsnack(object sender, EventArgs e)
        {
            // Naviguer vers la nouvelle page
            await Navigation.PushAsync(new choixsnack());
        }
        private async void profil(object sender, EventArgs e)
        {
            // Naviguer vers la nouvelle page
            await Navigation.PushAsync(new profil());
        }

        private async void choixrestau(object sender, EventArgs e)
        {
            // Naviguer vers la nouvelle page
            await Navigation.PushAsync(new choixrestau());
        }
        private async void choixcaffe(object sender, EventArgs e)
        {
            // Naviguer vers la nouvelle page
            await Navigation.PushAsync(new choixcaffe());
        }
        private async void OnCompassClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
        }
        private async void home(object sender, EventArgs e)
        {
            // Naviguer vers la nouvelle page
            await Navigation.PushAsync(new MainPage());
        }
    }
}
