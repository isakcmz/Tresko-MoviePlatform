using Microsoft.AspNetCore.Identity;

namespace FilmSitesi.Web.Models.Entities;

public class AppUser : IdentityUser
{
    public List<Review> Reviews { get; set; } = new();

    public List<WatchlistItem> WatchlistItems { get; set; } = new();

    public List<WatchedMovie> WatchedMovies { get; set; } = new();

    public List<Activity> Activities { get; set; } = new();

    public List<FavoriteMovie> FavoriteMovies { get; set; } = new();
}