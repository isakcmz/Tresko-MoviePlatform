namespace FilmSitesi.Web.Models.ViewModels;

public class WatchedMovieViewModel
{
    public int MovieId { get; set; }

    public int TmdbId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? PosterPath { get; set; }

    public DateTime WatchedDate { get; set; }

    public string Notes { get; set; } = string.Empty;
}