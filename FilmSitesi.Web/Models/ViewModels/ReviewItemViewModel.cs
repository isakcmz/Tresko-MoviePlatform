namespace FilmSitesi.Web.Models.ViewModels;

public class ReviewItemViewModel
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Comment { get; set; } = string.Empty;

    public double Rating { get; set; }

    public DateTime CreatedAt { get; set; }

    public int LikeCount { get; set; }

    public bool IsLikedByCurrentUser { get; set; }
}