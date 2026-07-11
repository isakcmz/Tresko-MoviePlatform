using FilmSitesi.Web.Data;
using FilmSitesi.Web.Models.Entities;
using FilmSitesi.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmSitesi.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public ProfileController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        var watchedCount = await _context.WatchedMovies
            .CountAsync(w => w.UserId == user.Id);

        var watchlistCount = await _context.WatchlistItems
            .CountAsync(w => w.UserId == user.Id);

        var reviewCount = await _context.Reviews
            .CountAsync(r => r.UserId == user.Id && !string.IsNullOrWhiteSpace(r.Comment));

        var recentWatched = await _context.WatchedMovies
            .Include(w => w.Movie)
            .Where(w => w.UserId == user.Id)
            .OrderByDescending(w => w.WatchedDate)
            .Take(5)
            .ToListAsync();

        var recentReviews = await _context.Reviews
            .Include(r => r.Movie)
            .Where(r => r.UserId == user.Id && !string.IsNullOrWhiteSpace(r.Comment))
            .OrderByDescending(r => r.CreatedAt)
            .Take(5)
            .ToListAsync();

        var recentActivities = await _context.Activities
            .Include(a => a.Movie)
            .Where(a => a.UserId == user.Id)
            .OrderByDescending(a => a.CreatedAt)
            .Take(10)
            .Select(a => new ActivityViewModel
            {
                UserName = user.UserName ?? "",
                MovieTitle = a.Movie.Title,
                MovieTmdbId = a.Movie.TmdbId,
                MoviePosterPath = a.Movie.PosterPath,
                Type = a.Type,
                Rating = a.Rating,
                Note = a.Note,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();
        

        var model = new ProfileViewModel
        {
            UserName = user.UserName ?? "",
            WatchedCount = watchedCount,
            WatchlistCount = watchlistCount,
            ReviewCount = reviewCount,
            RecentWatched = recentWatched,
            RecentReviews = recentReviews,
            RecentActivities = recentActivities
        };

        return View(model);
    }
}