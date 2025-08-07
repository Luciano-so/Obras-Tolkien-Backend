using TerraMedia.Application.Dtos.OpenLibrary;

namespace TerraMedia.Application.Interfaces;

public interface IOpenLibraryService
{
    Task<OpenLibrarySearchDto> SearchBooksAsync(string? author, int page = 1, int limit = 10, CancellationToken cancellationToken = default);
    Task<OpenLibraryAuthorDto?> GetAuthorBioAsync(string authorKey, CancellationToken cancellationToken = default);
}
