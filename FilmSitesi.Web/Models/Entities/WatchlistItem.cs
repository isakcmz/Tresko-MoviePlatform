namespace FilmSitesi.Web.Models.Entities;

public class WatchlistItem
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public AppUser User { get; set; } = null!;

    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int Priority { get; set; } = 3;

    public string Notes { get; set; } = string.Empty;
}