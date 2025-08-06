using TerraMedia.Application.Mappings;

namespace TerraMedia.Api.Configuration;
public static class AutoMapperConfig
{
    public static void AutoMapperRegister(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<BookMappingProfile>();
            cfg.AddProfile<BookCommentMappingProfile>();
        });
    }
}
