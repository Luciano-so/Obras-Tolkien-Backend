using System.Text.Json;
using TerraMedia.Application.Dtos.OpenLibrary;

namespace TerraMedia.Application.Tests.Dtos.OpenLibrary;
public class OpenLibrarySearchDtoTests
{
    [Fact]
    public void Deserialize_OpenLibrarySearchDto_ShouldMapPropertiesCorrectly()
    {
        var json = @"{
            ""NumFound"": 2,
            ""Start"": 0,
            ""Docs"": [
                {
                    ""Title"": ""The Hobbit"",
                    ""Author_name"": [""J.R.R. Tolkien""],
                    ""First_publish_year"": 1937,
                    ""Edition_count"": 5,
                    ""Cover_i"": 12345,
                    ""Key"": ""/books/OL123456M""
                },
                {
                    ""Title"": ""The Lord of the Rings"",
                    ""Author_name"": [""J.R.R. Tolkien""],
                    ""First_publish_year"": 1954,
                    ""Edition_count"": 10,
                    ""Cover_i"": 54321,
                    ""Key"": ""/books/OL654321M""
                }
            ]
        }";

        var searchDto = JsonSerializer.Deserialize<OpenLibrarySearchDto>(json);

        Assert.NotNull(searchDto);
        Assert.Equal(2, searchDto.NumFound);
        Assert.Equal(0, searchDto.Start);
        Assert.NotNull(searchDto.Docs);
        Assert.Equal(2, searchDto.Docs.Count);
        Assert.Equal("The Hobbit", searchDto.Docs[0].Title);
        Assert.Equal("The Lord of the Rings", searchDto.Docs[1].Title);
    }

    [Fact]
    public void Serialize_OpenLibrarySearchDto_ShouldGenerateCorrectJson()
    {
        var searchDto = new OpenLibrarySearchDto
        {
            NumFound = 1,
            Start = 0,
            Docs = new List<OpenLibraryBookDto>
            {
                new OpenLibraryBookDto
                {
                    Title = "The Hobbit",
                    Author_name = new List<string> { "J.R.R. Tolkien" },
                    First_publish_year = 1937,
                    Edition_count = 5,
                    Cover_i = 12345,
                    Key = "/books/OL123456M"
                }
            }
        };

        var json = JsonSerializer.Serialize(searchDto);

        Assert.Contains("\"NumFound\":1", json);
        Assert.Contains("\"Start\":0", json);
        Assert.Contains("\"Docs\":[", json);
        Assert.Contains("\"Title\":\"The Hobbit\"", json);
    }
}