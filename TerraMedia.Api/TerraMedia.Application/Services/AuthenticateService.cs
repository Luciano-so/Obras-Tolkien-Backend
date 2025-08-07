using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Interfaces;
using TerraMedia.Domain.Contracts.IRepositories;
using TerraMedia.Domain.Extensions;
using TerraMedia.Domain.Settings;

namespace TerraMedia.Application.Services;

public class AuthenticateService : IAuthenticateService
{
    private readonly IUserRepository _userRepository;
    private readonly AuthorizeSettings _authorizeSettings;

    public AuthenticateService(IUserRepository userRepository, IOptions<AuthorizeSettings> options)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _authorizeSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<TokenDto> AuthenticateAsync(AuthenticateDto dto)
    {
        var encryptedPassword = dto.Password.Encrypt();
        var user = await _userRepository.Authenticate(dto.Login, encryptedPassword);

        if (user == null)
            throw new UnauthorizedAccessException("Usuário ou senha inválido.");

        var claims = CreateClaims(user.Id, user.Name);
        var token = GenerateToken(claims);

        return new TokenDto { AccessToken = token };
    }

    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authorizeSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_authorizeSettings.Expires),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static IEnumerable<Claim> CreateClaims(Guid userId, string userName)
    {
        return new List<Claim>
        {
            new Claim("version", Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0"),
            new Claim("userId", userId.ToString()),
            new Claim("userName", userName)
        };
    }
}
