namespace FilmSitesi.Web.Models.Entities;

public class WatchedMovie
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public AppUser User { get; set; } = null!;

    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public DateTime WatchedDate { get; set; } = DateTime.UtcNow;

    public string Notes { get; set; } = string.Empty;
}