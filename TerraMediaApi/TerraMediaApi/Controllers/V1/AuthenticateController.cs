using Microsoft.AspNetCore.Mvc;
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
    private readonly IAuthenticateService _authenticateService;

    public AuthenticateController(IAuthenticateService authenticateService) => _authenticateService = authenticateService;


    [HttpPost("Authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateDto authenticateDto)
    {
        var result = await _authenticateService.AuthenticateAsync(authenticateDto);
        return Ok(ReponseDto.Create(HttpStatusCode.OK, "Usuario autenticado com sucesso", result));
    }
}
