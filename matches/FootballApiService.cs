using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menu
{
    public class FootballApiService
    {
        private readonly HttpClient _httpClient;

        public FootballApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://v3.football.api-sports.io/")
            };
            _httpClient.DefaultRequestHeaders.Add("x-apisports-key", "a3c01e569e6f1362f6684e91d2dbde15");
        }

        public async Task<string> GetTodayMatchesAsync()
        {
            string today = DateTime.UtcNow.ToString("yyyy-MM-dd"); // au format requis par l’API
            var response = await _httpClient.GetAsync($"fixtures?date={today}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
