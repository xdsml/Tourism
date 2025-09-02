using Firebase.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;


namespace menu.Pages;

public partial class SignInViewModel : ObservableObject
{
    private readonly FirebaseAuthClient _authClient;


    [ObservableProperty]
    private string? _email;

    [ObservableProperty]
    private string? _password;

    [ObservableProperty]
    private string? _errorMessage; // New property for error messages

    [ObservableProperty]
    private string? googleToken;



    public SignInViewModel(FirebaseAuthClient authClient)
    {
        _authClient = authClient;
        Email = string.Empty;
        Password = string.Empty;
    }


    [RelayCommand]
    private async Task SignIn()
    {
        try
        {
            var user = await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);

            if (user != null)
            {
                Preferences.Set("uid", user.User.Uid);
                Console.WriteLine($"✅ Connexion réussie - UID : {user.User.Uid}");


                // Redirection vers la page principale
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
        catch (FirebaseAuthException ex)
        {
            Console.WriteLine($"🔥 FirebaseAuthException : {ex.Reason}");

            if (ex.Reason == AuthErrorReason.WrongPassword)
            {
                ErrorMessage = "Mot de passe incorrect.";
            }
            else if (ex.Reason == AuthErrorReason.UnknownEmailAddress || ex.Reason == AuthErrorReason.UserNotFound)
            {
                ErrorMessage = "Aucun compte trouvé avec cet email.";
            }
            else if (ex.Reason == AuthErrorReason.InvalidEmailAddress)
            {
                ErrorMessage = "Veuillez entrer une adresse email valide.";
            }
            else
            {
                ErrorMessage = "Erreur d'authentification.";
            }

            // ❌ À SUPPRIMER — cette ligne écrase tout :
            // ErrorMessage = ex.Message;
        }
    }

    /* public async Task SignInWithGoogle()
     {
         try
         {
             // Récupérer le Token OAuth2 via navigateur
             GoogleSignInHelper googleSignInHelper = new();
             googleToken = await googleSignInHelper.GetGoogleTokenAsync();

             if (!string.IsNullOrEmpty(googleToken))
             {
                 var googleProvider = new GoogleProvider();
                 var credential = GoogleProvider.GetCredential(googleToken);
                 var authResult = await _authClient.SignInWithCredentialAsync(credential);

                 if (authResult != null)
                 {
                     var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken);
                     Debug.WriteLine($"Google Auth User: {payload.Email}");

                     await Shell.Current.GoToAsync("//WelcomePage");
                 }
             }
         }
         catch (Exception ex)
         {
             Debug.WriteLine($"Google Sign-In Error: {ex.Message}");
             ErrorMessage = "Failed to sign in with Google.";
         }
     }
    */

    public ICommand NavigateSignUpTapGesture => new Command(async () =>
    {
        await Shell.Current.GoToAsync("//SignUp");
    });
    public void ResetFields()
    {
        Email = string.Empty;
        Password = string.Empty;
        ErrorMessage = string.Empty;
    }

}