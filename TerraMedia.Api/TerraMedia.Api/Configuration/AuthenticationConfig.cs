using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TerraMedia.Api.Configuration;

public static class AuthenticationConfig
{
    public static void AuthenticationRegister(this IServiceCollection services, IConfiguration configuration)
    {
        var authorizeSettings = configuration.GetSection("AuthorizeSettings");
        var secret = authorizeSettings.GetSection("Secret").Value ?? "Chave";

        var key = Encoding.ASCII.GetBytes(secret);
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    public static void AuthenticationRegister(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}