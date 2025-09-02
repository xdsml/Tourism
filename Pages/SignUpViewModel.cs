using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Database;
using System.Windows.Input;
using System.Text.RegularExpressions;

using Firebase.Database.Query;
using System.ComponentModel.DataAnnotations;

namespace menu.Pages;

public partial class SignUpViewModel : ObservableObject
{
    private readonly FirebaseAuthClient _authClient;
    private readonly FirebaseClient _firebaseClient;
    
    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? username;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? nom;

    [ObservableProperty]
    private string? prenom;

    [ObservableProperty]
    private string? _errorMessage;

    public SignUpViewModel(FirebaseAuthClient authClient)
    {
        _authClient = authClient;

        _firebaseClient = new FirebaseClient("https://guidsign-c2579-default-rtdb.europe-west1.firebasedatabase.app/");
    }

    
    [RelayCommand]
   
    private async Task SignUp()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Email) || !IsValidEmail(Email))
            {
                ErrorMessage = "Veuillez entrer une adresse email valide.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
            {
                ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.";
                return;
            }

            var result = await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password);
            var uid = result.User.Uid;

            await _firebaseClient
                .Child("users")
                .Child(uid)
                .PutAsync(new
                {
                    Nom = this.Nom,
                    Prenom = this.Prenom,
                    Email = this.Email
                });

            await _firebaseClient
                .Child("scores")
                .Child(uid)
                .PutAsync(0);

            Preferences.Set("uid", uid);
            await Shell.Current.GoToAsync("//SignIn");
        }
        catch (FirebaseAuthException ex) when (ex.Message.Contains("WEAK_PASSWORD"))
        {
            ErrorMessage = "Mot de passe faible";
        }
        catch (Exception ex)
        {
            ErrorMessage = "Inscription échouée ";
        }
    }




    public ICommand NavigateSignInCommand => new Command(async () =>
    {
        await Shell.Current.GoToAsync("//SignIn");
    });
    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

}
