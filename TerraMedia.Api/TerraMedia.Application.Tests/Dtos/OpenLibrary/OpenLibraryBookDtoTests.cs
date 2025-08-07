using System.Text.Json;
using TerraMedia.Application.Dtos.OpenLibrary;

namespace TerraMedia.Application.Tests.Dtos.OpenLibrary;
public class OpenLibraryBookDtoTests
{
    [Fact]
    public void Deserialize_OpenLibraryBookDto_ShouldMapPropertiesCorrectly()
    {
        var json = @"{
            ""Title"": ""The Hobbit"",
            ""Author_name"": [""J.R.R. Tolkien""],
            ""Authors"": ""J.R.R. Tolkien"",
            ""First_publish_year"": 1937,
            ""Edition_count"": 5,
            ""Cover_i"": 12345,
            ""CoverUrl"": ""http://example.com/cover.jpg"",
            ""Cover_edition_key"": ""OL123456M"",
            ""Author_key"": [""OL1A""],
            ""Key"": ""/books/OL123456M""
        }";

        var book = JsonSerializer.Deserialize<OpenLibraryBookDto>(json);

        Assert.NotNull(book);
        Assert.Equal("The Hobbit", book.Title);
        Assert.NotNull(book.Author_name);
        Assert.Contains("J.R.R. Tolkien", book.Author_name);
        Assert.Equal("J.R.R. Tolkien", book.Authors);
        Assert.Equal(1937, book.First_publish_year);
        Assert.Equal(5, book.Edition_count);
        Assert.Equal(12345, book.Cover_i);
        Assert.Equal("http://example.com/cover.jpg", book.CoverUrl);
        Assert.Equal("OL123456M", book.Cover_edition_key);
        Assert.NotNull(book.Author_key);
        Assert.Contains("OL1A", book.Author_key);
        Assert.Equal("/books/OL123456M", book.Key);
    }

    [Fact]
    public void Serialize_OpenLibraryBookDto_ShouldGenerateCorrectJson()
    {
        var book = new OpenLibraryBookDto
        {
            Title = "The Hobbit",
            Author_name = new List<string> { "J.R.R. Tolkien" },
            Authors = "J.R.R. Tolkien",
            First_publish_year = 1937,
            Edition_count = 5,
            Cover_i = 12345,
            CoverUrl = "http://example.com/cover.jpg",
            Cover_edition_key = "OL123456M",
            Author_key = new List<string> { "OL1A" },
            Key = "/books/OL123456M"
        };

        var json = JsonSerializer.Serialize(book);

        Assert.Contains("\"Title\":\"The Hobbit\"", json);
        Assert.Contains("\"Author_name\":[\"J.R.R. Tolkien\"]", json);
        Assert.Contains("\"Authors\":\"J.R.R. Tolkien\"", json);
        Assert.Contains("\"First_publish_year\":1937", json);
        Assert.Contains("\"Edition_count\":5", json);
        Assert.Contains("\"Cover_i\":12345", json);
        Assert.Contains("\"CoverUrl\":\"http://example.com/cover.jpg\"", json);
        Assert.Contains("\"Cover_edition_key\":\"OL123456M\"", json);
        Assert.Contains("\"Author_key\":[\"OL1A\"]", json);
        Assert.Contains("\"Key\":\"/books/OL123456M\"", json);
    }
}
