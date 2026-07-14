namespace FilmSitesi.Web.Models.Entities;

public class UserFollow
{
    public int Id { get; set; }

    public string FollowerId { get; set; } = string.Empty;

    public string FollowedId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public AppUser Follower { get; set; } = null!;

    public AppUser Followed { get; set; } = null!;
}