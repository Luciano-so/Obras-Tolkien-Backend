using System.Diagnostics.CodeAnalysis;
using TerraMedia.Application.Interfaces;
using TerraMedia.Application.Services;
using TerraMedia.Domain.Contracts.IRepositories;
using TerraMedia.ExternalServices.OpenLibrary.Services;
using TerraMedia.Infrastructure.Persistence.Repositories;
using TerraMedia.Integration.ExternalServices.OpenLibrary.Clients;

namespace TerraMedia.Api.Configuration;

[ExcludeFromCodeCoverage]
public static class DependencyConfig
{
    public static void DependencyRegister(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthenticateService, AuthenticateService>();
        services.AddScoped<IBookCommentRulesService, BookCommentRulesService>();
        services.AddScoped<IOpenLibraryService, OpenLibraryService>();

        services.AddHttpClient<IOpenLibraryClient, OpenLibraryClient>();
    }
}
