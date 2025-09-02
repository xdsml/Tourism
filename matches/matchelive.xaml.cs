
using Newtonsoft.Json;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Graphics;
namespace menu.matches;

public partial class matchelive : ContentPage
{
    private FootballApiService _footballService = new FootballApiService();

    public matchelive()
	{
		InitializeComponent();
        LoadScores();

    }
    private async void home(object sender, EventArgs e)
    {
        // Naviguer vers la nouvelle page
        await Navigation.PushAsync(new MainPage());
    }

   
    private async void LoadScores()
    {
        try
        {
            var json = await _footballService.GetTodayMatchesAsync();
            var result = JsonConvert.DeserializeObject<FootballResponse>(json);

            // 🔍 Ne garder que les matchs de la Premier League
            var premierLeagueMatches = result.Response
                .Where(f => f.League?.Name == "Premier League")
                .Select(f => new
                {
                    HomeTeamName = f.Teams.Home.Name,
                    AwayTeamName = f.Teams.Away.Name,
                    Score = (f.Goals.Home.HasValue && f.Goals.Away.HasValue)
                        ? $"{f.Goals.Home} - {f.Goals.Away}"
                        : "À venir",
                    HomeTeamLogo = f.Teams.Home.Logo,
                    AwayTeamLogo = f.Teams.Away.Logo,
                    MatchStatusIcon = GetStatusIcon(f),
                    MatchStatusColor = GetStatusColor(f)
                })
                .ToList();

            ScoresList.ItemsSource = premierLeagueMatches;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Impossible de charger les scores : {ex.Message}", "OK");
        }
    }


    private string GetStatusIcon(FootballFixture match)
    {
        // tu peux adapter selon les propriétés de l’API
        return match.Goals.Home.HasValue && match.Goals.Away.HasValue ? "🔴" : "⚪";
    }

    private Color GetStatusColor(FootballFixture match)
    {
        return match.Goals.Home.HasValue && match.Goals.Away.HasValue ? Colors.Red : Colors.Gray;
    }
    private async void OnCompassClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChoixPhotoPage()); // Ouvre ta page pour publier
    }
    private async void profil(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new profil()); // Ouvre ta page pour publier
    }

}
