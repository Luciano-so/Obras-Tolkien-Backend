using TerraMedia.Domain.Settings;

namespace TerraMedia.Tests.Domain.Settings;

public class OpenLibrarySettingsTests
{
    [Fact]
    public void Should_Set_Properties_Correctly()
    {
        var settings = new OpenLibrarySettings
        {
            BaseUrl = "https://openlibrary.org",
            CoverBaseUrl = "https://covers.openlibrary.org"
        };

        Assert.Equal("https://openlibrary.org", settings.BaseUrl);
        Assert.Equal("https://covers.openlibrary.org", settings.CoverBaseUrl);
    }

    [Fact]
    public void Should_Have_Default_Values()
    {
        var settings = new OpenLibrarySettings();

        Assert.Equal(string.Empty, settings.BaseUrl);
        Assert.Equal(string.Empty, settings.CoverBaseUrl);
    }
}
