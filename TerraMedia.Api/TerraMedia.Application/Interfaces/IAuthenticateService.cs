using TerraMedia.Application.Dtos;

namespace TerraMedia.Application.Interfaces;

public interface IAuthenticateService
{
    Task<TokenDto> AuthenticateAsync(AuthenticateDto autenticar);
}
