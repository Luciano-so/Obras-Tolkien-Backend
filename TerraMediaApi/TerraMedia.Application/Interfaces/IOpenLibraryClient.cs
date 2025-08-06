using TerraMedia.Application.Dtos.OpenLibrary;

namespace TerraMedia.Application.Interfaces;

public interface IOpenLibraryClient
{
    Task<OpenLibrarySearchDto?> SearchBooksAsync(string? author, string? title, int page, int limit);
    string? GetCoverUrl(string? coverEditionKey, string size = "M");
}
