using System.Text.Json;
using FilmSitesi.Web.Models.Dtos;

namespace FilmSitesi.Web.Services;

public class TmdbService : ITmdbService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public TmdbService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    }

    public async Task<List<TmdbMovieSearchItemDto>> SearchMoviesAsync(string query)
    {
        var apiKey = _configuration["Tmdb:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new Exception("TMDb API key bulunamadı.");

        var url = $"search/movie?api_key={apiKey}&query={Uri.EscapeDataString(query)}&language=tr-TR";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return new List<TmdbMovieSearchItemDto>();

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<TmdbSearchResponseDto>(json, options);

        return result?.Results ?? new List<TmdbMovieSearchItemDto>();
    }

    public async Task<TmdbMovieDetailDto?> GetMovieDetailsAsync(int tmdbId)
    {
        var apiKey = _configuration["Tmdb:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new Exception("TMDb API key bulunamadı.");

        var url = $"movie/{tmdbId}?api_key={apiKey}&language=tr-TR";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<TmdbMovieDetailDto>(json, options);
    }
}