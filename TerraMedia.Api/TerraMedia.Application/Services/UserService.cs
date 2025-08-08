using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Exceptions;
using TerraMedia.Application.Interfaces;
using TerraMedia.Domain.Contracts.IRepositories;
using TerraMedia.Domain.Entities;
using TerraMedia.Domain.Extensions;
using TerraMedia.Domain.Settings;

namespace TerraMedia.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly AuthorizeSettings _authorizeSettings;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IOptions<AuthorizeSettings> options, IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _authorizeSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<TokenDto> AuthenticateAsync(AuthenticateDto dto)
    {
        var encryptedPassword = dto.Password.Encrypt();
        var user = await _userRepository.Authenticate(dto.Login, encryptedPassword) ??
            throw new UnauthorizedAccessException("Usuário ou senha inválido.");

        var claims = CreateClaims(user.Id, user.Name);
        var token = GenerateToken(claims);

        return new TokenDto { AccessToken = token };
    }

    public async Task CreateUserAsync(CreateUserDto dto)
    {
        if (await _userRepository.ExistsUser(dto.Login))
            throw new AppException("Já existe um usuário com este login.");

        var user = _mapper.Map<User>(dto);
        user.Validate();
        user.Encrypt();
        await _userRepository.AddAsync(user);
        await _userRepository.UnitOfWork.Commit();
    }

    private async Task<User> GetUserOrThrowAsync(Guid id)
    {
        var user = await _userRepository.FindAsync(id);
        return user ?? throw new AppException("Usuário não encontrado.");
    }

    public async Task UpdatePasswordAsync(UpdatePasswordDto dto)
    {
        var user = await GetUserOrThrowAsync(dto.Id);

        user.ValidPassword(dto.Password);
        user.Encrypt();

        _userRepository.Update(user);
        await _userRepository.UnitOfWork.Commit();
    }

    public async Task UpdateUserAsync(UpdateUserDto dto)
    {
        var user = await GetUserOrThrowAsync(dto.Id);

        user.ChangeStatus(dto.Status);

        _userRepository.Update(user);
        await _userRepository.UnitOfWork.Commit();
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<List<UserDto>>(users);
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
