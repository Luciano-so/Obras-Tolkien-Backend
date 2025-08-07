using TerraMedia.Domain.Settings;

namespace TerraMedia.Api.Configuration;

public static class AppSettingsConfig
{
    public static void AppSettingsRegister(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthorizeSettings>(configuration.GetSection("AuthorizeSettings"));
        services.Configure<OpenLibrarySettings>(configuration.GetSection("OpenLibrary")); ;
    }
}
