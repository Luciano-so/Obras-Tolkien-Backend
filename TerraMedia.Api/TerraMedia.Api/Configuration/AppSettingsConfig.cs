using System.Diagnostics.CodeAnalysis;
using TerraMedia.Domain.Settings;

namespace TerraMedia.Api.Configuration;
[ExcludeFromCodeCoverage]
public static class AppSettingsConfig
{
    public static void AppSettingsRegister(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthorizeSettings>(configuration.GetSection("AuthorizeSettings"));
        services.Configure<OpenLibrarySettings>(configuration.GetSection("OpenLibrary")); ;
    }
}
