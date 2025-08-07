using System.Text.Json.Serialization;
using TerraMedia.Application.Converters;

namespace TerraMedia.Application.Dtos.OpenLibrary;

public class OpenLibraryAuthorDto
{
    public string? Name { get; set; }
    public string? Fuller_name { get; set; }
    public string? Birth_date { get; set; }
    public string? Death_date { get; set; }
    [JsonPropertyName("bio")]
    [JsonConverter(typeof(BioConverter))]
    public string? Bio { get; set; }
}
