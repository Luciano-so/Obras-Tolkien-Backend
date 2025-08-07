using Newtonsoft.Json;

namespace TerraMedia.Api.Configuration;

public static class ApplicationConfig
{
    public static void ApplicationRegister(this IServiceCollection services)
    {
        services.AddControllersWithViews().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });
    }
}
