

public class Post
{
    public string ImageUrl { get; set; } = string.Empty;  // Valeur par d�faut
    public string? VideoUrl { get; set; }
    public string Description { get; set; } = string.Empty;  // Valeur par d�faut
    public int PostId { get; set; }
    public string UserId { get; set; } = string.Empty;  // Valeur par d�faut
    public string MediaType { get; set; } = string.Empty;  // Valeur par d�faut
}
