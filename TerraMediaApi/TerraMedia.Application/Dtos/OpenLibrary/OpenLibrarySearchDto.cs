namespace TerraMedia.Application.Dtos.OpenLibrary;
public class OpenLibrarySearchDto
{
    public int NumFound { get; set; }
    public int Start { get; set; }
    public List<OpenLibraryBookDto> Docs { get; set; } = new();
}
