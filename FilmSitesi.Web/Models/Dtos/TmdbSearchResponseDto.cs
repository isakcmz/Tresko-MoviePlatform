using System.Text.Json.Serialization;

namespace FilmSitesi.Web.Models.Dtos;

public class TmdbSearchResponseDto
{
    [JsonPropertyName("results")]
    public List<TmdbMovieSearchItemDto> Results { get; set; } = new();
}