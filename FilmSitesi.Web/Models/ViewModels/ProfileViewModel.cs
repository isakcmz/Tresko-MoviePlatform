using FilmSitesi.Web.Models.Entities;

namespace FilmSitesi.Web.Models.ViewModels;

public class ProfileViewModel
{
    public string UserName { get; set; } = string.Empty;

    public int WatchedCount { get; set; }

    public int WatchlistCount { get; set; }

    public int ReviewCount { get; set; }

    public List<WatchedMovie> RecentWatched { get; set; } = new();

    public List<Review> RecentReviews { get; set; } = new();
    
    public List<ActivityViewModel> RecentActivities { get; set; } = new();
}