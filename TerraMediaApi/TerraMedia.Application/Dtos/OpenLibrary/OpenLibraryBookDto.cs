namespace TerraMedia.Application.Dtos.OpenLibrary;

public class OpenLibraryBookDto
{
    public string? Title { get; set; }
    public List<string>? Author_name { get; set; }
    public string? Authors { get; set; }
    public int First_publish_year { get; set; }
    public int Edition_count { get; set; }
    public int Cover_i { get; set; }
    public string? CoverUrl { get; set; }
    public string? Cover_edition_key { get; set; }
}
