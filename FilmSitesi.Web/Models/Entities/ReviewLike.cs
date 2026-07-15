namespace FilmSitesi.Web.Models.Entities;

public class ReviewLike
{
    public int Id { get; set; }

    public int ReviewId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public Review Review { get; set; } = null!;

    public AppUser User { get; set; } = null!;
}