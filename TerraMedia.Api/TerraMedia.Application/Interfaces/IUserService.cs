using System.Threading.Tasks;
using TerraMedia.Application.Dtos;

namespace TerraMedia.Application.Interfaces;

public interface IUserService
{
    Task<TokenDto> AuthenticateAsync(AuthenticateDto autenticar);
    Task CreateUserAsync(CreateUserDto dto);
    Task UpdatePasswordAsync(UpdatePasswordDto dto);
    Task UpdateUserAsync(UpdateUserDto dto);
    Task<List<UserDto>> GetAllUsersAsync();
}
