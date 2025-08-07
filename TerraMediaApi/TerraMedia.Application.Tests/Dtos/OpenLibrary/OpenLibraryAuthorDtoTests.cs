using System.Text.Json;
using TerraMedia.Application.Dtos.OpenLibrary;

namespace TerraMedia.Application.Tests.Dtos.OpenLibrary;

public class OpenLibraryAuthorDtoTests
{
    [Fact]
    public void Deserialize_WithBioConverter_ShouldDeserializeCorrectly()
    {
        var json = @"{
            ""Name"": ""J.R.R. Tolkien"",
            ""Fuller_name"": ""John Ronald Reuel Tolkien"",
            ""Birth_date"": ""1892-01-03"",
            ""Death_date"": ""1973-09-02"",
            ""bio"": { ""value"": ""Famous author of fantasy novels."" }
        }";

        var author = JsonSerializer.Deserialize<OpenLibraryAuthorDto>(json);

        Assert.NotNull(author);
        Assert.Equal("J.R.R. Tolkien", author.Name);
        Assert.Equal("John Ronald Reuel Tolkien", author.Fuller_name);
        Assert.Equal("1892-01-03", author.Birth_date);
        Assert.Equal("1973-09-02", author.Death_date);
        Assert.Equal("Famous author of fantasy novels.", author.Bio);
    }

    [Fact]
    public void Serialize_WithBioConverter_ShouldSerializeCorrectly()
    {
        var author = new OpenLibraryAuthorDto
        {
            Name = "J.R.R. Tolkien",
            Fuller_name = "John Ronald Reuel Tolkien",
            Birth_date = "1892-01-03",
            Death_date = "1973-09-02",
            Bio = "Famous author of fantasy novels."
        };

        var json = JsonSerializer.Serialize(author);

        Assert.Contains("\"bio\"", json);
        Assert.Contains("Famous author of fantasy novels.", json);
    }
}
