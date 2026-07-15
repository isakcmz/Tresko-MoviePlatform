using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FilmSitesi.Web.Models;
using FilmSitesi.Web.Data;
using FilmSitesi.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using FilmSitesi.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace FilmSitesi.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<AppUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        var followingIds = new List<string>();

        if (currentUser != null)
        {
            followingIds = await _context.UserFollows
                .Where(f => f.FollowerId == currentUser.Id)
                .Select(f => f.FollowedId)
                .ToListAsync();
        }

        var query = _context.Activities
            .Include(a => a.User)
            .Include(a => a.Movie)
            .AsQueryable();

        if (currentUser != null)
        {
            query = query.Where(a => a.UserId != currentUser.Id);

            if (followingIds.Any())
            {
                query = query.Where(a => followingIds.Contains(a.UserId));
            }
        }

        var activities = await query
            .OrderByDescending(a => a.CreatedAt)
            .Take(20)
            .Select(a => new ActivityViewModel
            {
                UserId = a.UserId,
                UserName = a.User.UserName ?? "",
                MovieTitle = a.Movie.Title,
                MovieTmdbId = a.Movie.TmdbId,
                MoviePosterPath = a.Movie.PosterPath,
                Type = a.Type,
                Rating = a.Rating,
                Note = a.Note,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        ViewBag.IsFollowingFeed = currentUser != null && followingIds.Any();

        return View(activities);
    }





    public IActionResult Privacy()
    {
        return View();
    }





    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
