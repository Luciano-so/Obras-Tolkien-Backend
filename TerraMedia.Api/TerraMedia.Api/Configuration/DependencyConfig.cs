using System.Diagnostics.CodeAnalysis;
using TerraMedia.Application.Interfaces;
using TerraMedia.Application.Services;
using TerraMedia.Domain.Contracts.IRepositories;
using TerraMedia.ExternalServices.OpenLibrary.Services;
using TerraMedia.Infrastructure.Persistence.Repositories;
using TerraMedia.Integration.ExternalServices.OpenLibrary.Clients;
using Microsoft.Extensions.Options;
using TerraMedia.Domain.Settings;

namespace TerraMedia.Api.Configuration;

[ExcludeFromCodeCoverage]
public static class DependencyConfig
{
    public static void DependencyRegister(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBookCommentRulesService, BookCommentRulesService>();
        services.AddScoped<IOpenLibraryService, OpenLibraryService>();

        services.AddHttpClient<IOpenLibraryClient, OpenLibraryClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<OpenLibrarySettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
        })
        .ConfigurePrimaryHttpMessageHandler(sp =>
        {
            var env = sp.GetRequiredService<IHostEnvironment>();
            if (env.IsDevelopment())
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            }
            else
            {
                return new HttpClientHandler();
            }
        });

    }
}
