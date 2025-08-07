using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using TerraMedia.Application.Dtos.OpenLibrary;
using TerraMedia.Application.Interfaces;
using TerraMedia.Domain.Settings;

namespace TerraMedia.Integration.ExternalServices.OpenLibrary.Clients;

public class OpenLibraryClient : IOpenLibraryClient
{
    private readonly HttpClient _httpClient;
    private readonly OpenLibrarySettings _settings;

    public OpenLibraryClient(HttpClient httpClient, IOptions<OpenLibrarySettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.BaseUrl);
        _settings = settings.Value;
    }

    public async Task<OpenLibrarySearchDto?> SearchBooksAsync(string? author, int page, int limit, CancellationToken cancellationToken = default)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(author))
            queryParams.Add($"author={Uri.EscapeDataString(author)}");

        queryParams.Add($"page={page}");
        queryParams.Add($"limit={limit}");

        var endpoint = $"/search.json?{string.Join("&", queryParams)}";

        return await _httpClient.GetFromJsonAsync<OpenLibrarySearchDto>(endpoint, cancellationToken);
    }

    public async Task<OpenLibraryAuthorDto?> GetAuthorBioAsync(string authorKey, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(authorKey))
            return null;

        var endpoint = $"/authors/{Uri.EscapeDataString(authorKey)}.json";

        return await _httpClient.GetFromJsonAsync<OpenLibraryAuthorDto>(endpoint, cancellationToken);
    }

    public string? GetCoverUrl(string? coverEditionKey, string size = "M")
    {
        if (string.IsNullOrEmpty(coverEditionKey))
            return null;

        size = size.ToUpper();
        if (size != "S" && size != "M" && size != "L")
            size = "M";

        return $"{_settings.CoverBaseUrl}{coverEditionKey}-{size}.jpg";
    }
}
