namespace FilmSitesi.Web.Models.Entities;

public class FavoriteMovie
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public AppUser User { get; set; } = null!;

    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}