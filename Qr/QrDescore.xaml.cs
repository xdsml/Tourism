using System.Text;
using Firebase.Database;
using Firebase.Database.Query;
using ZXing.Net.Maui;

namespace menu.Qr;

public partial class QrDescore : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private bool _isHandlingScan = false;

    public QrDescore()
    {
        try
        {
            Console.WriteLine("🚀 Avant InitializeComponent");
            InitializeComponent();
            Console.WriteLine("✅ Après InitializeComponent");

            _firebaseClient = new FirebaseClient("https://guidsign-c2579-default-rtdb.europe-west1.firebasedatabase.app/");
            Console.WriteLine("✅ Firebase client prêt");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Erreur dans QrDescore : " + ex.Message);
            Console.WriteLine("📄 StackTrace : " + ex.StackTrace);
        }
    }

    private async void barcodeReaderDescore_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (_isHandlingScan) return; // 🔒 Ignore si déjà en traitement
        _isHandlingScan = true;

        try
        {
            var first = e.Results?.FirstOrDefault();
            if (first is null)
            {
                _isHandlingScan = false;
                return;
            }

            barcodeReaderDescore.IsDetecting = false;

            string qrCode = first.Value;
            string safeQrCodeKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(qrCode));
            string? uid = Preferences.Get("uid", null);

            if (string.IsNullOrEmpty(uid))
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                    DisplayAlert("Erreur", "Utilisateur non connecté.", "OK"));
                return;
            }

            // 🔐 Vérifie si QR autorisé
            var validRef = _firebaseClient.Child("valid_qrdescore").Child(safeQrCodeKey);
            bool isValid = await validRef.OnceSingleAsync<bool?>() ?? false;

            if (!isValid)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                    DisplayAlert("❌ Non", "QR code non autorisé pour bénéficier d'une réduction", "OK"));
                return;
            }

            // 🕒 Vérifie si déjà scanné
            var scanRef = _firebaseClient.Child("qrdescore_scans").Child(uid).Child(safeQrCodeKey);
            var lastScan = await scanRef.OnceSingleAsync<DateTime?>();

            if (lastScan.HasValue)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                    DisplayAlert("⏳ Déjà scanné", "Ce QR code a déjà été utilisé pour bénéficier d'une réduction.", "OK"));
                return;
            }

            // ✅ Premier scan → on stocke la date
            await scanRef.PutAsync(DateTime.UtcNow);

            // ➖ Décrémentation du score
            var scoreRef = _firebaseClient.Child("scores").Child(uid);
            int score = await scoreRef.OnceSingleAsync<int?>() ?? 0;

            if (score > 0)
            {
                await scoreRef.PutAsync(score - 1);
                await MainThread.InvokeOnMainThreadAsync(() =>
                    DisplayAlert("📤 Décrémenté", $"Nouveau score : {score - 1}", "OK"));
            }
            else
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                    DisplayAlert("ℹ️ Score bas", "Le score est déjà à zéro.", "OK"));
            }
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
            barcodeReaderDescore.IsDetecting = true; // 🔥 Redémarre la détection
            _isHandlingScan = false; // 🔓 Permet de traiter un nouveau QR
        }
    }

    private async void OnCloseTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}
