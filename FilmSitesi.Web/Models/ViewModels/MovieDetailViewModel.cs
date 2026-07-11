using FilmSitesi.Web.Models.Entities;

namespace FilmSitesi.Web.Models.ViewModels;

public class MovieDetailViewModel
{
    public Movie Movie { get; set; } = null!;

    public double? UserRating { get; set; }

    public string UserComment { get; set; } = string.Empty;

    public List<Review> Reviews { get; set; } = new();

    public bool IsInWatchlist { get; set; }

    public int WatchlistPriority { get; set; } = 3;

    public string WatchlistNotes { get; set; } = string.Empty;

    public bool HasWatched { get; set; }

    public int WatchedCount { get; set; }

    public string WatchedNotes { get; set; } = string.Empty;

    public double AverageRating { get; set; }

    public int RatingCount { get; set; }
}