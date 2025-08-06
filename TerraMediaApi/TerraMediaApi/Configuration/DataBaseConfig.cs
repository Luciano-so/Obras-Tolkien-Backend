using Microsoft.EntityFrameworkCore;
using TerraMedia.Infrastructure.Persistence.Contexts;

namespace TerraMedia.Api.Configuration;

public static class DataBaseConfig
{
    public static void DataBaseRegister(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MainContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void DataBaseRegister(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<MainContext>();
        dbContext.Database.Migrate();
    }
}
