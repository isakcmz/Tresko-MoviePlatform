namespace FilmSitesi.Web.Models.ViewModels;

public class UserListItemViewModel
{
    public string UserId { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public int FollowersCount { get; set; }

    public int FollowingCount { get; set; }

    public bool IsFollowing { get; set; }
}