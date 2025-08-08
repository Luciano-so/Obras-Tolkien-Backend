using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Interfaces;

namespace TerraMedia.Api.Controllers.V1;
/// <summary>
/// Version 1.0 Controller
/// </summary>
[Route("api/v1/[controller]")]
public class AuthenticateController : MainController
{
    private readonly IUserService _userService;

    public AuthenticateController(IUserService userService) => _userService = userService;

    [HttpPost("Authenticate")]
    [SwaggerOperation(Summary = "Autentica o usuário", Description = "Recebe login e senha, retorna token JWT para acesso autenticado")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Usuário autenticado com sucesso", typeof(ResponseDto<TokenDto>))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Credenciais inválidas")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateDto authenticateDto)
    {
        var result = await _userService.AuthenticateAsync(authenticateDto);
        return Ok(ResponseDto.Create(HttpStatusCode.OK, "Usuario autenticado com sucesso", result));
    }

    [HttpPost("Create")]
    [SwaggerOperation(Summary = "Cria um novo usuário", Description = "Cadastra um novo usuário com nome, login e senha")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Usuário criado com sucesso")]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        await _userService.CreateUserAsync(dto);
        return Created(string.Empty, ResponseDto.Create(HttpStatusCode.Created, "Usuário criado com sucesso"));
    }

    [HttpPatch("Password")]
    [SwaggerOperation(Summary = "Atualiza a senha do usuário", Description = "Atualiza a senha de um usuário existente")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Senha atualizada com sucesso")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
    {
        await _userService.UpdatePasswordAsync(dto);
        return Ok(ResponseDto.Create(HttpStatusCode.OK, "Senha atualizada com sucesso"));
    }

    [HttpPatch("Status")]
    [SwaggerOperation(Summary = "Altera o status do usuário", Description = "Ativa ou desativa o usuário")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Status do usuário atualizado com sucesso")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateUserDto dto)
    {
        await _userService.UpdateUserAsync(dto);
        return Ok(ResponseDto.Create(HttpStatusCode.OK, "Status do usuário atualizado com sucesso"));
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Lista todos os usuários", Description = "Retorna todos os usuários cadastrados (sem senha)")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Lista de usuários retornada com sucesso", typeof(ResponseDto<List<UserDto>>))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(ResponseDto.Create(HttpStatusCode.OK, "Usuários retornados com sucesso", result));
    }
}
