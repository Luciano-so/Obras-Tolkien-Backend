using TerraMedia.Domain.Settings;

namespace TerraMedia.Integration.Tests.ExternalServices.OpenLibrary.Settings;

public class OpenLibrarySettingsTest
{
    [Fact]
    public void Should_Set_Properties_Correctly()
    {
        var settings = new OpenLibrarySettings
        {
            BaseUrl = "URL",
            CoverBaseUrl = "https://covers.openlibrary.org/b/",
        };

        Assert.Equal("URL", settings.BaseUrl);
        Assert.Equal("https://covers.openlibrary.org/b/", settings.CoverBaseUrl);
    }

    [Fact]
    public void Default_Values_Should_Be_Set()
    {
        var settings = new OpenLibrarySettings();

        Assert.Equal(string.Empty, settings.BaseUrl);
        Assert.Equal(string.Empty, settings.CoverBaseUrl);
    }
}
