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

    [Authorize]
    public async Task<IActionResult> Index(string? userId)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
            return Challenge();

        AppUser? profileUser;

        if (string.IsNullOrWhiteSpace(userId))
        {
            profileUser = currentUser;
        }
        else
        {
            profileUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (profileUser == null)
                return NotFound();
        }

        var watchedCount = await _context.WatchedMovies
            .CountAsync(w => w.UserId == profileUser.Id);

        var watchlistCount = await _context.WatchlistItems
            .CountAsync(w => w.UserId == profileUser.Id);

        var reviewCount = await _context.Reviews
            .CountAsync(r => r.UserId == profileUser.Id && !string.IsNullOrWhiteSpace(r.Comment));

        var followersCount = await _context.UserFollows
            .CountAsync(f => f.FollowedId == profileUser.Id);

        var followingCount = await _context.UserFollows
            .CountAsync(f => f.FollowerId == profileUser.Id);

        var isOwnProfile = currentUser.Id == profileUser.Id;

        var isFollowing = false;
        if (!isOwnProfile)
        {
            isFollowing = await _context.UserFollows
                .AnyAsync(f => f.FollowerId == currentUser.Id && f.FollowedId == profileUser.Id);
        }

        var recentWatched = await _context.WatchedMovies
            .Include(w => w.Movie)
            .Where(w => w.UserId == profileUser.Id)
            .OrderByDescending(w => w.WatchedDate)
            .Take(5)
            .ToListAsync();

        var recentReviews = await _context.Reviews
            .Include(r => r.Movie)
            .Where(r => r.UserId == profileUser.Id && !string.IsNullOrWhiteSpace(r.Comment))
            .OrderByDescending(r => r.CreatedAt)
            .Take(5)
            .ToListAsync();

        var recentActivities = await _context.Activities
            .Include(a => a.Movie)
            .Where(a => a.UserId == profileUser.Id)
            .OrderByDescending(a => a.CreatedAt)
            .Take(10)
            .Select(a => new ActivityViewModel
            {
                UserId = a.UserId,
                UserName = profileUser.UserName ?? "",
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
            UserId = profileUser.Id,
            UserName = profileUser.UserName ?? "",
            IsOwnProfile = isOwnProfile,
            IsFollowing = isFollowing,
            FollowersCount = followersCount,
            FollowingCount = followingCount,
            WatchedCount = watchedCount,
            WatchlistCount = watchlistCount,
            ReviewCount = reviewCount,
            RecentWatched = recentWatched,
            RecentReviews = recentReviews,
            RecentActivities = recentActivities
        };

        return View(model);
    }


    




    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Follow(string userId)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
            return Challenge();

        if (currentUser.Id == userId)
            return RedirectToAction("Index", new { userId });

        var targetUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (targetUser == null)
            return NotFound();

        var existing = await _context.UserFollows
            .FirstOrDefaultAsync(f => f.FollowerId == currentUser.Id && f.FollowedId == userId);

        if (existing == null)
        {
            var follow = new UserFollow
            {
                FollowerId = currentUser.Id,
                FollowedId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserFollows.Add(follow);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", new { userId });
    }






    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Unfollow(string userId)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
            return Challenge();

        var follow = await _context.UserFollows
            .FirstOrDefaultAsync(f => f.FollowerId == currentUser.Id && f.FollowedId == userId);

        if (follow != null)
        {
            _context.UserFollows.Remove(follow);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", new { userId });
    }



}