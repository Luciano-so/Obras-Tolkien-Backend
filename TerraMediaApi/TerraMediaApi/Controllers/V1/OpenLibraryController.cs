using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Dtos.OpenLibrary;
using TerraMedia.Application.Interfaces;

namespace TerraMedia.Api.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class OpenLibraryController : ControllerBase
{
    private readonly IOpenLibraryService _service;
    public OpenLibraryController(IOpenLibraryService service) => _service = service;

    [HttpGet("search")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Busca livros por autor",
                      Description = "Busca livros na OpenLibrary filtrando por autor com paginação.")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Busca realizada com sucesso", typeof(ReponseDto<OpenLibrarySearchDto>))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Parâmetros inválidos")]
    public async Task<IActionResult> Search([FromQuery] string? author, [FromQuery] int page = 1, [FromQuery] int limit = 10, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(author))
            return BadRequest(ReponseDto.Create(HttpStatusCode.BadRequest, "É necessário informar o autor."));

        var result = await _service.SearchBooksAsync(author, page, limit, cancellationToken);
        return Ok(ReponseDto.Create(HttpStatusCode.OK, "Busca realizada com sucesso", result));
    }

    [HttpGet("{authorKey}/bio")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Busca a biografia do autor",
        Description = "Obtém a biografia do autor a partir da chave (authorKey) na OpenLibrary."
    )]
    [SwaggerResponse((int)HttpStatusCode.OK, "Biografia obtida com sucesso", typeof(ReponseDto<string>))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Chave do autor inválida")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Biografia não encontrada para o autor informado")]
    public async Task<IActionResult> GetAuthorBio(string authorKey, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(authorKey))
            return BadRequest(ReponseDto.Create(HttpStatusCode.BadRequest, "É necessário informar a chave do autor."));

        var result = await _service.GetAuthorBioAsync(authorKey, cancellationToken);

        if (result == null)
            return NotFound(ReponseDto.Create(HttpStatusCode.NotFound, "Biografia não encontrada para o autor informado."));

        return Ok(ReponseDto.Create(HttpStatusCode.OK, "Biografia obtida com sucesso", result));
    }
}
