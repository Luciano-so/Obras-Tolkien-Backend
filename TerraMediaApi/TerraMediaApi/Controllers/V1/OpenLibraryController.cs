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
    public async Task<IActionResult> Search([FromQuery] string? author, [FromQuery] string? title, [FromQuery] int page = 1, [FromQuery] int limit = 10)
    {
        if (string.IsNullOrWhiteSpace(author) && string.IsNullOrWhiteSpace(title))
            return BadRequest(ReponseDto.Create(HttpStatusCode.BadRequest, "É necessário informar o autor ou o título."));

        var result = await _service.SearchBooksAsync(author, title, page, limit);
        return Ok(ReponseDto.Create(HttpStatusCode.OK, "Busca realizada com sucesso", result));
    }
}
