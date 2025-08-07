
using TerraMedia.Integration.ExternalServices.OpenLibrary.Settings;

namespace TerraMedia.Integration.Tests.ExternalServices.OpenLibrary.Settings;

public class OpenLibrarySettingsTest
{
    [Fact]
    public void Should_Set_Properties_Correctly()
    {
        var settings = new OpenLibrarySettings
        {
            BaseUrl = "URL",
        };

        Assert.Equal("URL", settings.BaseUrl);
    }

    [Fact]
    public void Default_Values_Should_Be_Set()
    {
        var settings = new OpenLibrarySettings();

        Assert.Equal(string.Empty, settings.BaseUrl);
    }
}
