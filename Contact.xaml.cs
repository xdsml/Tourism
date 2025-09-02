using System.Net.NetworkInformation;
namespace menu;

using Microsoft.Maui.ApplicationModel.Communication;
using System.Diagnostics;

public partial class Contact : ContentPage
{
    public Contact()
    {
        InitializeComponent();
    }

    private async void OnEnvoyerClicked(object sender, EventArgs e)
    {
        string subject = SujetEntry.Text?.Trim();
        string body = MessageEditor.Text?.Trim();
        string recipient = "momed.mokhtari28@gmail.com"; // Votre email ici

        // Validation des champs
        if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
        {
            await DisplayAlert("Erreur", "Veuillez remplir tous les champs.", "OK");
            return;
        }

        try
        {
            // Essayer d'abord avec l'URI mailto (méthode universelle)
            var mailtoUri = $"mailto:{recipient}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";

            var canOpen = await Launcher.Default.CanOpenAsync(mailtoUri);

            if (canOpen)
            {
                await Launcher.Default.OpenAsync(mailtoUri);
                return;
            }

            // Fallback: Utiliser l'API Email de MAUI
            if (Email.Default.IsComposeSupported)
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = new List<string> { recipient }
                };
                await Email.Default.ComposeAsync(message);
            }
            else
            {
                // Si tout échoue, proposer d'installer Gmail
                bool installGmail = await DisplayAlert(
                    "Client email requis",
                    "Aucun client email détecté. Voulez-vous installer Gmail?",
                    "Oui", "Non");

                if (installGmail)
                {
                    await Launcher.Default.OpenAsync(
                        "https://play.google.com/store/apps/details?id=com.google.android.gm");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur",
                $"Impossible d'envoyer l'email: {ex.Message}",
                "OK");
        }
    }
    private async void OnretourClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage()); // Ouvre ta page pour publier
    }
    

}