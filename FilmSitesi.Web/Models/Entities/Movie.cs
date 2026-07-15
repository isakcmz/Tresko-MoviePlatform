namespace FilmSitesi.Web.Models.Entities;

public class Movie
{
    public int Id { get; set; }

    public int TmdbId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string OriginalTitle { get; set; } = string.Empty;

    public string Overview { get; set; } = string.Empty;

    public DateTime? ReleaseDate { get; set; }

    public string? PosterPath { get; set; }

    public string? BackdropPath { get; set; }

    public double VoteAverage { get; set; }

    public string OriginalLanguage { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Review> Reviews { get; set; } = new();

    public List<WatchlistItem> WatchlistItems { get; set; } = new();

    public List<WatchedMovie> WatchedMovies { get; set; } = new();

    public List<Activity> Activities { get; set; } = new();
}