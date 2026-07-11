using FilmSitesi.Web.Models.Dtos;

namespace FilmSitesi.Web.Models.ViewModels;

public class MovieSearchViewModel
{
    public string Query { get; set; } = "";

    public List<TmdbMovieSearchItemDto> Results { get; set; } = new();
}