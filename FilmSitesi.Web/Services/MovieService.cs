using FilmSitesi.Web.Data;
using FilmSitesi.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FilmSitesi.Web.Services;

public class MovieService : IMovieService
{
    private readonly AppDbContext _context;
    private readonly ITmdbService _tmdbService;

    public MovieService(AppDbContext context, ITmdbService tmdbService)
    {
        _context = context;
        _tmdbService = tmdbService;
    }

    public async Task<Movie?> GetOrCreateMovieAsync(int tmdbId)
    {
        // 1️⃣ önce veritabanına bak
        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.TmdbId == tmdbId);

        if (movie != null)
            return movie;

        // 2️⃣ API'den çek
        var tmdbMovie = await _tmdbService.GetMovieDetailsAsync(tmdbId);

        if (tmdbMovie == null)
            return null;

        // 3️⃣ entity oluştur
        movie = new Movie
        {
            TmdbId = tmdbMovie.Id,
            Title = tmdbMovie.Title ?? string.Empty,
            OriginalTitle = tmdbMovie.OriginalTitle ?? string.Empty,
            Overview = tmdbMovie.Overview ?? string.Empty,
            PosterPath = tmdbMovie.PosterPath,
            BackdropPath = tmdbMovie.BackdropPath,
            VoteAverage = tmdbMovie.VoteAverage,
            OriginalLanguage = tmdbMovie.OriginalLanguage ?? string.Empty
        };

        // 4️⃣ veritabanına kaydet
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return movie;
    }
}