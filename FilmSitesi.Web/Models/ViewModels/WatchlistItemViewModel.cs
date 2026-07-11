namespace FilmSitesi.Web.Models.ViewModels;

public class WatchlistItemViewModel
{
    public int MovieId { get; set; }

    public int TmdbId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;

    public int Priority { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}