using Firebase.Database;
using Firebase.Database.Query;
using Google.Cloud.Storage.V1;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Firebase.Auth;
using System.Reactive.Linq;

namespace menu;

public partial class profil : ContentPage
{
    public readonly FirebaseClient _firebaseClient = new("https://guidsign-c2579-default-rtdb.europe-west1.firebasedatabase.app/");
    public readonly FirebaseAuthClient _authClient;

    public profil()
    {
        InitializeComponent();
        LoadUserInfo(); // Charge automatiquement au lancement
    }

    public async void LoadUserInfo()
    {
        try
        {
            string uid = Preferences.Get("uid", null);
            Console.WriteLine($"🔥 UID actuel : {uid}");

            if (string.IsNullOrEmpty(uid))
            {
                await DisplayAlert("Non connecté", "Aucun utilisateur connecté. Veuillez vous authentifier.", "OK");
                return;
            }

            // Lire les informations de l'utilisateur
            var userData = await _firebaseClient
                .Child("users")
                .Child(uid)
                .OnceSingleAsync<UserModel>();

            // Lire le score dans la branche /scores
            var scoreData = await _firebaseClient
                .Child("scores")
                .Child(uid)
                .OnceSingleAsync<int>();  // Le score est un entier direct

            if (userData != null)
            {
                nomLabel.Text = userData.Nom ?? "Nom inconnu";
                prenomLabel.Text = userData.Prenom ?? "Prénom inconnu";
                profilImage.Source = !string.IsNullOrEmpty(userData.ImgUrl)
                    ? ImageSource.FromUri(new Uri(userData.ImgUrl))
                    : "default_avatar.png";

                scoreLabel.Text = $"Score: {scoreData}";  // 🛠 Affiche le vrai score ici
            }
            else
            {
                await DisplayAlert("Erreur", "Aucun utilisateur trouvé dans la base de données.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", "Impossible de charger le profil.", "OK");
            Console.WriteLine($"💥 Erreur Firebase : {ex.Message}");
        }
    }



    public async void LoadUserData(string uid)
    {
        try
        {
            var userData = await _firebaseClient
                .Child("users")
                .Child(uid)
                .OnceSingleAsync<UserModel>();

            if (userData != null)
            {
                // Sauvegarder nom et prénom dans les préférences
                Preferences.Set("nom", userData.Nom ?? "");
                Preferences.Set("prenom", userData.Prenom ?? "");

                nomLabel.Text = userData.Nom;
                prenomLabel.Text = userData.Prenom;
                scoreLabel.Text = $"Score: {userData.Score}";

                if (!string.IsNullOrEmpty(userData.ImgUrl))
                {
                    profilImage.Source = ImageSource.FromUri(new Uri(userData.ImgUrl));
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", "Erreur de chargement initial", "OK");
        }
    }





    private async void home(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        LogoutPopup.IsVisible = true;
    }

    private void CancelLogout_Clicked(object sender, EventArgs e)
    {
        LogoutPopup.IsVisible = false;
    }

    private async void ConfirmLogout_Clicked(object sender, EventArgs e)
    {
        LogoutPopup.IsVisible = false;
        Preferences.Remove("uid");
        Preferences.Remove("email");
        Preferences.Remove("password");
        await Shell.Current.GoToAsync("//SignIn");
    }

    public async Task<string> UploadImageToCloudinary(string localFilePath, string fileName)
    {
        var account = new Account(
            "dvujlwv6u",
            "975766743984623",
            "Flu_T_anjOxbxOTv07A2qzWpXm0"
        );

        var cloudinary = new Cloudinary(account);

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(localFilePath),
            PublicId = fileName,
            Overwrite = true
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        return uploadResult.SecureUrl?.ToString() ?? string.Empty;
    }

    private async void OnUploadImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Choisissez une image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                string localPath = result.FullPath;
                string fileName = Path.GetFileNameWithoutExtension(result.FileName);
                string uid = Preferences.Get("uid", null);

                if (string.IsNullOrEmpty(uid)) return;

                // 🔎 Récupère les infos actuelles de l'utilisateur
                var currentUser = await _firebaseClient
                    .Child("users")
                    .Child(uid)
                    .OnceSingleAsync<UserModel>();

                // 📤 Upload de l'image
                var newImageUrl = await UploadImageToCloudinary(localPath, fileName);

                // ❌ Évite la duplication si la même image est déjà présente
                if (currentUser?.ImgUrl == newImageUrl)
                {
                    await DisplayAlert("Info", "Cette image est déjà utilisée dans votre profil.", "OK");
                    return;
                }

                // ✅ Met à jour l'image dans Firebase
                await _firebaseClient
                    .Child("users")
                    .Child(uid)
                    .PatchAsync(new { ImgUrl = newImageUrl });

                // 🔁 Rafraîchis l'affichage dans l'app
                profilImage.Source = ImageSource.FromUri(new Uri(newImageUrl));

                await DisplayAlert("Succès", "Image mise à jour avec succès !", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Une erreur s’est produite : {ex.Message}", "OK");
        }
    }



double currentScale = 1;
    double startScale = 1;
    double xOffset = 0;
    double yOffset = 0;

    private void OnImagePinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Started)
        {
            startScale = profilImage.Scale;
            profilImage.AnchorX = 0;
            profilImage.AnchorY = 0;
        }
        if (e.Status == GestureStatus.Running)
        {
            currentScale = startScale * e.Scale;
            profilImage.Scale = Math.Max(1, currentScale);
        }
    }
    private async void OnCompassClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
    }


    private void OnImagePanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                profilImage.TranslationX = xOffset + e.TotalX;
                profilImage.TranslationY = yOffset + e.TotalY;
                break;
            case GestureStatus.Completed:
                xOffset = profilImage.TranslationX;
                yOffset = profilImage.TranslationY;
                break;
        }
    }
}


public class UserModel
{
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Email { get; set; }
    public string ImgUrl { get; set; }
    public int Score { get; set; }  // 🆕 Ajouté ici !!
}
