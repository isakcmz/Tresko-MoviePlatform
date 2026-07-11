using FilmSitesi.Web.Models.Entities;

namespace FilmSitesi.Web.Services;

public interface IMovieService
{
    Task<Movie?> GetOrCreateMovieAsync(int tmdbId);
}