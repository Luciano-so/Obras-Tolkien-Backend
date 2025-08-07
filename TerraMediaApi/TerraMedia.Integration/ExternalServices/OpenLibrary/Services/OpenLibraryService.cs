using TerraMedia.Application.Dtos.OpenLibrary;
using TerraMedia.Application.Interfaces;

namespace TerraMedia.ExternalServices.OpenLibrary.Services;

public class OpenLibraryService : IOpenLibraryService
{
    private readonly IOpenLibraryClient _client;

    public OpenLibraryService(IOpenLibraryClient client)
    {
        _client = client;
    }

    public async Task<OpenLibrarySearchDto> SearchBooksAsync(string? author, int page = 1, int limit = 10, CancellationToken cancellationToken = default)
    {
        var result = await _client.SearchBooksAsync(author, page, limit, cancellationToken);

        if (result?.Docs == null)
            return new OpenLibrarySearchDto();

        foreach (var book in result.Docs)
        {
            book.CoverUrl = _client.GetCoverUrl(book.Cover_edition_key);
            book.Authors = book.Author_name != null ? string.Join(", ", book.Author_name) : "Autor desconhecido";
            book.Key = book.Author_key != null ? string.Join(", ", book.Author_key) : "Chave desconhecida";
        }
        return result;
    }

    public async Task<OpenLibraryAuthorDto?> GetAuthorBioAsync(string authorKey, CancellationToken cancellationToken = default)
    {
        var result = await _client.GetAuthorBioAsync(authorKey, cancellationToken);

        if (result == null)
            return new OpenLibraryAuthorDto();

        return result;
    }
}
