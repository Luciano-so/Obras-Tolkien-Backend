using TerraMedia.Application.Dtos.OpenLibrary;

namespace TerraMedia.Application.Interfaces;

public interface IOpenLibraryService
{
    Task<OpenLibrarySearchDto> SearchBooksAsync(string author, string title, int page = 1, int limit = 10);
}
