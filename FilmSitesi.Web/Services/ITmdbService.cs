using FilmSitesi.Web.Models.Dtos;

namespace FilmSitesi.Web.Services;

public interface ITmdbService
{
    Task<List<TmdbMovieSearchItemDto>> SearchMoviesAsync(string query);
    Task<TmdbMovieDetailDto?> GetMovieDetailsAsync(int tmdbId);
}