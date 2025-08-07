using TerraMedia.Application.Dtos.OpenLibrary;

namespace TerraMedia.Application.Interfaces;

public interface IOpenLibraryClient
{
    Task<OpenLibrarySearchDto?> SearchBooksAsync(string? author, int page, int limit, CancellationToken cancellationToken = default);
    Task<OpenLibraryAuthorDto?> GetAuthorBioAsync(string authorKey, CancellationToken cancellationToken = default);
    string? GetCoverUrl(string? coverEditionKey, string size = "M");
}
