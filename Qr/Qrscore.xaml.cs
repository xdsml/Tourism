using System.Text;
using Firebase.Database;
using Firebase.Database.Query;
using ZXing.Net.Maui;


namespace menu.Qr;

public partial class Qrscore : ContentPage
{
    private readonly FirebaseClient _firebaseClient;

    public Qrscore()
    {
        try
        {
            Console.WriteLine("🚀 Avant InitializeComponent");
            InitializeComponent(); // 💥 S’il plante ici, le XAML est invalide
            Console.WriteLine("✅ Après InitializeComponent");

            _firebaseClient = new FirebaseClient("https://guidsign-c2579-default-rtdb.europe-west1.firebasedatabase.app/");
            Console.WriteLine("✅ Firebase client prêt");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Erreur dans Qrscore : " + ex.Message);
            Console.WriteLine("📄 StackTrace : " + ex.StackTrace);
        }
    }


    private bool _isHandlingScan = false;

    private async void barcodeReader_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        try
        {
            var first = e.Results?.FirstOrDefault();
            if (first is null)
                return;

            barcodeReader.IsDetecting = false;

            string qrCode = first.Value;
            string safeQrCodeKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(qrCode));
            string? uid = Preferences.Get("uid", null);

            if (string.IsNullOrEmpty(uid))
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                    DisplayAlert("Erreur", "Utilisateur non connecté.", "OK"));
                return;
            }

            // 🔐 Vérifie si QR autorisé
            var validRef = _firebaseClient.Child("valid_qrcodes").Child(safeQrCodeKey);
            bool isValid = await validRef.OnceSingleAsync<bool?>() ?? false;

            if (!isValid)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                    DisplayAlert("❌ Non", "QR code non autorisé", "OK"));
                return;
            }

            // 🕒 Vérifie si déjà scanné
            var scanRef = _firebaseClient.Child("scans").Child(uid).Child(safeQrCodeKey);
            var lastScan = await scanRef.OnceSingleAsync<DateTime?>();

            if (lastScan.HasValue)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                    DisplayAlert("⏳ Déjà scanné", "Tu as déjà scanné ce QR code.", "OK"));
                return;
            }

            // ✅ Premier scan → on stocke la date
            await scanRef.PutAsync(DateTime.UtcNow);

            // ➕ Incrémentation du score
            var scoreRef = _firebaseClient.Child("scores").Child(uid);
            int score = await scoreRef.OnceSingleAsync<int?>() ?? 0;
            await scoreRef.PutAsync(score + 1);

            await MainThread.InvokeOnMainThreadAsync(() =>
                DisplayAlert("🎉 Bravo !", $"Score : {score + 1}", "OK"));
        }
        catch (Exception ex)
        {
            Console.WriteLine("💥 Exception : " + ex.Message);
            Console.WriteLine("📄 StackTrace : " + ex.StackTrace);
            await MainThread.InvokeOnMainThreadAsync(() =>
                DisplayAlert("💥 Erreur", ex.Message, "OK"));
        }
        finally
        {
            barcodeReader.IsDetecting = true;
        }
    }
    private async void OnDescoreNavigateClick(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//QrDescore");
    }
    private void OnQrIconTapped(object sender, EventArgs e)
    {
        qrIcon.IsVisible = false;
        decrementBtn.IsVisible = false;
        barcodeReader.IsVisible = true;
        closeScannerBtn.IsVisible = true;
        barcodeReader.IsDetecting = true;
    }

    private async void OnCloseScannerClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");

        qrIcon.IsVisible = true;
        decrementBtn.IsVisible = true;
        closeScannerBtn.IsVisible = false;
        barcodeReader.IsVisible = false;
        barcodeReader.IsDetecting = false;
    }
    private async void OnScanTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new menu.Qr.Qrscore());
    }
    private async void OnDecrementTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//QrDescore");

    }

}