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
        var currentUserId = currentUser?.Id;

        var query = _context.Activities
            .Include(a => a.User)
            .Include(a => a.Movie)
            .AsQueryable();

        if (!string.IsNullOrEmpty(currentUserId))
        {
            query = query.Where(a => a.UserId != currentUserId);
        }

        var activities = await query
            .OrderByDescending(a => a.CreatedAt)
            .Take(20)
            .Select(a => new ActivityViewModel
            {
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
