using FilmSitesi.Web.Models.ViewModels;
using FilmSitesi.Web.Services;
using FilmSitesi.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using FilmSitesi.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace FilmSitesi.Web.Controllers;

public class MoviesController : Controller
{
    private readonly IMovieService _movieService;
    private readonly ITmdbService _tmdbService;
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public MoviesController(IMovieService movieService, ITmdbService tmdbService, AppDbContext context, UserManager<AppUser> userManager)
    {
        _movieService = movieService;
        _tmdbService = tmdbService;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Detail(int id)
    {
        var movie = await _movieService.GetOrCreateMovieAsync(id);

        if (movie == null)
            return NotFound();

        var currentUser = await _userManager.GetUserAsync(User);
        var currentUserId = currentUser?.Id;

        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.MovieId == movie.Id)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReviewItemViewModel
            {
                Id = r.Id,
                UserName = r.User.UserName ?? "",
                Comment = r.Comment,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt,
                LikeCount = _context.ReviewLikes.Count(rl => rl.ReviewId == r.Id),
                IsLikedByCurrentUser = currentUserId != null &&
                                    _context.ReviewLikes.Any(rl => rl.ReviewId == r.Id && rl.UserId == currentUserId)
            })
            .ToListAsync();

        var averageRating = 0.0;
        var ratingCount = reviews.Count;

        if (ratingCount > 0)
        {
            averageRating = reviews.Average(r => r.Rating);
        }

        var viewModel = new MovieDetailViewModel
        {
            Movie = movie,
            Reviews = reviews,
            AverageRating = averageRating,
            RatingCount = ratingCount
        };

        var user = await _userManager.GetUserAsync(User);

        if (user != null)
        {
            var userReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == user.Id && r.MovieId == movie.Id);

            if (userReview != null)
            {
                viewModel.UserRating = userReview.Rating;
                viewModel.UserComment = userReview.Comment;
            }

            var watchlistItem = await _context.WatchlistItems
                .FirstOrDefaultAsync(w => w.UserId == user.Id && w.MovieId == movie.Id);

            if (watchlistItem != null)
            {
                viewModel.IsInWatchlist = true;
                viewModel.WatchlistPriority = watchlistItem.Priority;
                viewModel.WatchlistNotes = watchlistItem.Notes;
            }

            var watchedItems = await _context.WatchedMovies
                .Where(w => w.UserId == user.Id && w.MovieId == movie.Id)
                .OrderByDescending(w => w.WatchedDate)
                .ToListAsync();

            if (watchedItems.Any())
            {
                viewModel.HasWatched = true;
                viewModel.WatchedCount = watchedItems.Count;
                viewModel.WatchedNotes = watchedItems.First().Notes;
            }
        }

        return View(viewModel);
    }

    

    [HttpGet]
    public IActionResult Search()
    {
        return View(new MovieSearchViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Search(MovieSearchViewModel model)
    {
        if (!string.IsNullOrWhiteSpace(model.Query))
        {
            model.Results = await _tmdbService.SearchMoviesAsync(model.Query);
        }

        return View(model);
    }







    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddReview(int movieId, string rating, string comment)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null)
            return NotFound();

        // JS'den gelen (. veya , içeren) ondalık değerleri culture bağımsız parse ediyoruz
        double parsedRating = 0;
        bool isParsed = double.TryParse(rating, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out parsedRating);
        if (!isParsed)
        {
            isParsed = double.TryParse(rating, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out parsedRating);
        }

        if (!isParsed || parsedRating < 0.5 || parsedRating > 5 || parsedRating % 0.5 != 0)
        {
            TempData["ReviewError"] = "Puan 0.5 ile 5 arasında ve 0.5 artışlarla olmalıdır.";
            return RedirectToAction("Detail", new { id = movie.TmdbId });
        }

        var existingReview = await _context.Reviews
            .FirstOrDefaultAsync(r => r.UserId == user.Id && r.MovieId == movieId);

        if (existingReview != null)
        {
            existingReview.Rating = parsedRating;
            existingReview.Comment = comment ?? string.Empty;
            existingReview.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            var review = new Review
            {
                UserId = user.Id,
                MovieId = movieId,
                Rating = parsedRating,
                Comment = comment ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
        }

        var activity = new Activity
        {
            UserId = user.Id,
            MovieId = movieId,
            Type = "Review",
            Rating = parsedRating,
            Note = comment ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };

        _context.Activities.Add(activity);

        await _context.SaveChangesAsync();

        return RedirectToAction("Detail", new { id = movie.TmdbId });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToWatchlist(int movieId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null)
            return NotFound();

        var existingItem = await _context.WatchlistItems
            .FirstOrDefaultAsync(w => w.UserId == user.Id && w.MovieId == movieId);

        if (existingItem == null)
        {
            var item = new WatchlistItem
            {
                UserId = user.Id,
                MovieId = movieId,
                CreatedAt = DateTime.UtcNow
            };

            _context.WatchlistItems.Add(item);
        

            var activity = new Activity
            {
                UserId = user.Id,
                MovieId = movieId,
                Type = "Watchlist",
                CreatedAt = DateTime.UtcNow
            };

            _context.Activities.Add(activity);

            await _context.SaveChangesAsync();
        }
        
        return RedirectToAction("Detail", new { id = movie.TmdbId });
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RemoveFromWatchlist(int movieId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null)
            return NotFound();

        var existingItem = await _context.WatchlistItems
            .FirstOrDefaultAsync(w => w.UserId == user.Id && w.MovieId == movieId);

        if (existingItem != null)
        {
            _context.WatchlistItems.Remove(existingItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Detail", new { id = movie.TmdbId });
    }



    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Watchlist()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        var items = await _context.WatchlistItems
            .Include(w => w.Movie)
            .Where(w => w.UserId == user.Id)
            .OrderBy(w => w.Priority)
            .ThenByDescending(w => w.CreatedAt)
            .Select(w => new WatchlistItemViewModel
            {
                MovieId = w.MovieId,
                TmdbId = w.Movie.TmdbId,
                Title = w.Movie.Title,
                PosterPath = w.Movie.PosterPath,
                Priority = w.Priority,
                Notes = w.Notes,
                CreatedAt = w.CreatedAt
            })
            .ToListAsync();

        return View(items);
    }



    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddWatched(int movieId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null)
            return NotFound();

        var watchedItem = new WatchedMovie
        {
            UserId = user.Id,
            MovieId = movieId,
            WatchedDate = DateTime.UtcNow
        };

        _context.WatchedMovies.Add(watchedItem);

        var activity = new Activity
        {
            UserId = user.Id,
            MovieId = movieId,
            Type = "Watched",
            CreatedAt = DateTime.UtcNow
        };

        _context.Activities.Add(activity);

        await _context.SaveChangesAsync();

        return RedirectToAction("Detail", new { id = movie.TmdbId });
    }



    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Watched()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        var items = await _context.WatchedMovies
            .Include(w => w.Movie)
            .Where(w => w.UserId == user.Id)
            .OrderByDescending(w => w.WatchedDate)
            .Select(w => new WatchedMovieViewModel
            {
                MovieId = w.MovieId,
                TmdbId = w.Movie.TmdbId,
                Title = w.Movie.Title,
                PosterPath = w.Movie.PosterPath,
                WatchedDate = w.WatchedDate,
                Notes = w.Notes
            })
            .ToListAsync();

        return View(items);
    }







    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ToggleLike(int reviewId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        var existing = await _context.ReviewLikes
            .FirstOrDefaultAsync(x => x.ReviewId == reviewId && x.UserId == user.Id);

        if (existing != null)
        {
            _context.ReviewLikes.Remove(existing);
        }
        else
        {
            var like = new ReviewLike
            {
                ReviewId = reviewId,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.ReviewLikes.Add(like);
        }

        await _context.SaveChangesAsync();

        return Redirect(Request.Headers["Referer"].ToString());
    }




}