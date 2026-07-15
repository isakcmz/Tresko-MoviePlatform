namespace FilmSitesi.Web.Models.ViewModels;

public class ActivityViewModel
{
    public string UserName { get; set; } = string.Empty;

    public string MovieTitle { get; set; } = string.Empty;

    public int MovieTmdbId { get; set; }

    public string? MoviePosterPath { get; set; }

    public string Type { get; set; } = string.Empty;

    public double? Rating { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public string UserId { get; set; } = string.Empty;
}