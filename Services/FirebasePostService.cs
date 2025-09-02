using System.Text;
using System.Text.Json;

namespace menu.Services
{
    public class FirebasePost
    {
        public string Id { get; set; } = "";
        public string? imageUrl { get; set; }
        public string PostOwner { get; set; } = "";
        public string? userId { get; set; }
        public long timestamp { get; set; }
        public int likes { get; set; }
        public int reports { get; set; }
        public int commentsCount { get; set; }
        public string? caption { get; set; }
        public string? userProfileImageUrl { get; set; } // Photo de profil
        public bool IsLiked;
        public Dictionary<string, FirebaseComment>? comments { get; set; }
        public Dictionary<string, FirebaseReport>? Reports { get; set; }
        public Dictionary<string, bool>? LikesUsers { get; set; }
    }

    public class FirebaseComment
    {
        public string? userId { get; set; }
        public string? content { get; set; }
        public long timestamp { get; set; }
    }

    public class FirebaseReport
    {
        public string? UserId { get; set; }
        public List<string> Reasons { get; set; }
        public long Timestamp { get; set; }
    }

    public class FirebasePostService
    {
        private readonly string _baseUrl = "https://guidsign-c2579-default-rtdb.europe-west1.firebasedatabase.app/";
        private readonly HttpClient client = new HttpClient();

        public async Task<string> AddPostAsync(FirebasePost post)
        {
            var json = JsonSerializer.Serialize(post);
            var response = await client.PostAsync($"{_baseUrl}/posts.json", new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erreur Firebase : " + await response.Content.ReadAsStringAsync());

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody);

            if (result != null && result.ContainsKey("name"))
            {
                string generatedId = result["name"];
                post.Id = generatedId;

                var updateJson = JsonSerializer.Serialize(new { Id = generatedId });
                await client.PatchAsync($"{_baseUrl}/posts/{generatedId}.json", new StringContent(updateJson, Encoding.UTF8, "application/json"));

                return generatedId;
            }

            throw new Exception("Impossible de récupérer l'ID généré.");
        }

        public async Task<Dictionary<string, FirebasePost>> GetAllPostsAsync()
        {
            var response = await client.GetAsync($"{_baseUrl}/posts.json");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erreur Firebase");

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, FirebasePost>>(json) ?? new();
        }

        public async Task<int> GetLikesAsync(string postId)
        {
            var response = await client.GetAsync($"{_baseUrl}/posts/{postId}/likes.json");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erreur Firebase récupération des likes");

            var json = await response.Content.ReadAsStringAsync();
            return int.TryParse(json, out var currentLikes) ? currentLikes : 0;
        }

        public async Task ToggleLikeAsync(string postId, string userId)
        {
            var likeCheck = await client.GetAsync($"{_baseUrl}/posts/{postId}/LikesUsers/{userId}.json");

            bool hasLiked = false;

            if (likeCheck.IsSuccessStatusCode)
            {
                var json = await likeCheck.Content.ReadAsStringAsync();
                hasLiked = json == "true";
            }

            if (hasLiked)
            {
                await client.DeleteAsync($"{_baseUrl}/posts/{postId}/LikesUsers/{userId}.json");

                int currentLikes = await GetLikesAsync(postId);
                int newLikes = Math.Max(currentLikes - 1, 0);

                var jsonUpdate = JsonSerializer.Serialize(new { likes = newLikes });
                await client.PatchAsync($"{_baseUrl}/posts/{postId}.json", new StringContent(jsonUpdate, Encoding.UTF8, "application/json"));
            }
            else
            {
                await client.PutAsync($"{_baseUrl}/posts/{postId}/LikesUsers/{userId}.json", new StringContent("true", Encoding.UTF8, "application/json"));

                int currentLikes = await GetLikesAsync(postId);
                int newLikes = currentLikes + 1;

                var jsonUpdate = JsonSerializer.Serialize(new { likes = newLikes });
                await client.PatchAsync($"{_baseUrl}/posts/{postId}.json", new StringContent(jsonUpdate, Encoding.UTF8, "application/json"));
            }
        }

        public async Task AddCommentAsync(string postId, FirebaseComment comment)
        {
            var json = JsonSerializer.Serialize(comment);
            var response = await client.PostAsync($"{_baseUrl}/posts/{postId}/comments.json", new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erreur Firebase ajout commentaire : " + await response.Content.ReadAsStringAsync());

            await IncrementCommentCountAsync(postId);
        }

        public async Task<Dictionary<string, FirebaseComment>> GetCommentsAsync(string postId)
        {
            var response = await client.GetAsync($"{_baseUrl}/posts/{postId}/comments.json");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erreur Firebase récupération commentaires");

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, FirebaseComment>>(json) ?? new();
        }

        public async Task IncrementCommentCountAsync(string postId)
        {
            var response = await client.GetAsync($"{_baseUrl}/posts/{postId}/commentsCount.json");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erreur récupération nombre de commentaires");

            var currentCountJson = await response.Content.ReadAsStringAsync();
            int currentCount = 0;

            if (!string.IsNullOrEmpty(currentCountJson) && currentCountJson != "null")
                currentCount = JsonSerializer.Deserialize<int>(currentCountJson);

            int newCount = currentCount + 1;

            var updateJson = JsonSerializer.Serialize(new { commentsCount = newCount });
            var updateResponse = await client.PatchAsync($"{_baseUrl}/posts/{postId}.json", new StringContent(updateJson, Encoding.UTF8, "application/json"));

            if (!updateResponse.IsSuccessStatusCode)
                throw new Exception("Erreur mise à jour nombre de commentaires");
        }

        public async Task AddReportAsync(string postId, string userId, List<string> reasons)
        {
            var report = new FirebaseReport
            {
                UserId = userId,
                Reasons = reasons,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            string reportKey = Guid.NewGuid().ToString("N");

            var reportJson = JsonSerializer.Serialize(report);

            var response = await client.PutAsync(
                $"{_baseUrl}/posts/{postId}/Reports/{reportKey}.json",
                new StringContent(reportJson, Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erreur ajout report : " + await response.Content.ReadAsStringAsync());
        }
    }
}
