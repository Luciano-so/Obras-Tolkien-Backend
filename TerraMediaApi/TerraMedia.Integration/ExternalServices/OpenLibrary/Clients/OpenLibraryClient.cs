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

    public async Task<OpenLibrarySearchDto?> SearchBooksAsync(string? author, string? title, int page, int limit)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(author))
            queryParams.Add($"author={Uri.EscapeDataString(author)}");

        if (!string.IsNullOrWhiteSpace(title))
            queryParams.Add($"title={Uri.EscapeDataString(title)}");

        queryParams.Add($"page={page}");
        queryParams.Add($"limit={limit}");

        var endpoint = $"/search.json?{string.Join("&", queryParams)}";

        return await _httpClient.GetFromJsonAsync<OpenLibrarySearchDto>(endpoint);
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
