using TerraMedia.Domain.Settings;

namespace TerraMedia.Tests.Domain.Settings
{
    public class AuthorizeSettingsTests
    {
        [Fact]
        public void Should_Set_Properties_Correctly()
        {
            var settings = new AuthorizeSettings
            {
                Secret = "my-secret-key",
                Expires = 60
            };

            Assert.Equal("my-secret-key", settings.Secret);
            Assert.Equal(60, settings.Expires);
        }

        [Fact]
        public void Default_Values_Should_Be_Set()
        {
            var settings = new AuthorizeSettings();

            Assert.Equal(string.Empty, settings.Secret);
            Assert.Equal(0, settings.Expires);
        }
    }
}
