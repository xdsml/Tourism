

public class Post
{
    public string ImageUrl { get; set; } = string.Empty;  // Valeur par défaut
    public string? VideoUrl { get; set; }
    public string Description { get; set; } = string.Empty;  // Valeur par défaut
    public int PostId { get; set; }
    public string UserId { get; set; } = string.Empty;  // Valeur par défaut
    public string MediaType { get; set; } = string.Empty;  // Valeur par défaut
}
