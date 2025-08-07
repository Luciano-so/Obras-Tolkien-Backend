using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Interfaces;

namespace TerraMedia.Api.Controllers.V1;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class BooksController : MainController
{
    private readonly IBookService _service;

    public BooksController(IBookService service) => _service = service;

    [Authorize]
    [HttpPost("create-with-comment/{coverId}")]
    [SwaggerOperation(Summary = "Cria um livro e adiciona um comentário",
                      Description = "Cria um novo livro com o CoverId informado e adiciona um comentário inicial.")]
    [SwaggerResponse((int)HttpStatusCode.Created, "Livro criado com comentário com sucesso", typeof(ReponseDto<BookDto>))]
    public async Task<IActionResult> CreateWithComment(int coverId, [FromBody] BookCommentDto commentDto)
    {
        var result = await _service.CreateBookWithCommentAsync(coverId, UserId, commentDto);
        return Ok(ReponseDto.Create(HttpStatusCode.Created, "Comentário cadastrado com sucesso", result));
    }

    [Authorize]
    [HttpPost("{coverId:guid}/add-comment")]
    [SwaggerOperation(Summary = "Adiciona um comentário a um livro existente",
                      Description = "Adiciona um novo comentário ao livro identificado pelo BookId.")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Comentário cadastrado com sucesso", typeof(ReponseDto<BookDto>))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Livro não encontrado")]
    public async Task<IActionResult> AddComment(int coverId, [FromBody] CommentDto commentDto)
    {
        var result = await _service.AddCommentToExistingBookAsync(coverId, UserId, commentDto);
        return Ok(ReponseDto.Create(HttpStatusCode.OK, "Comentário cadastrado com sucesso", result));
    }

    [Authorize]
    [HttpPut("{bookId:guid}/update-comment/{commentId:guid}")]
    [SwaggerOperation(Summary = "Atualiza um comentário de um livro",
                      Description = "Atualiza o comentário identificado pelo CommentId no livro especificado pelo BookId.")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Comentário atualizado com sucesso", typeof(ReponseDto<BookDto>))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Livro ou comentário não encontrado")]
    public async Task<IActionResult> UpdateComment(Guid bookId, Guid commentId, [FromBody] CommentDto commentDto)
    {
        var result = await _service.UpdateCommentAsync(bookId, UserId, commentId, commentDto);
        return Ok(ReponseDto.Create(HttpStatusCode.OK, "Comentário atualizado com sucesso", result));
    }

    [Authorize]
    [HttpDelete("{bookId:guid}/remove-comment/{commentId:guid}")]
    [SwaggerOperation(Summary = "Remove um comentário de um livro",
                      Description = "Remove o comentário identificado pelo CommentId no livro especificado pelo BookId.")]
    [SwaggerResponse((int)HttpStatusCode.NoContent, "Comentário removido com sucesso")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Livro ou comentário não encontrado")]
    public async Task<IActionResult> RemoveComment(Guid bookId, Guid commentId)
    {
        await _service.RemoveCommentAsync(bookId, commentId);
        return Ok(ReponseDto.Create(HttpStatusCode.NoContent, "Comentário removido com sucesso"));
    }

    [Authorize]
    [HttpGet("{bookId:guid}")]
    [SwaggerOperation(Summary = "Obtém um livro com seus comentários",
                      Description = "Retorna o livro identificado pelo BookId junto com seus comentários ativos.")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Informações carregadas com sucesso.", typeof(ReponseDto<BookDto>))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Livro não encontrado")]
    public async Task<IActionResult> GetBook(Guid bookId)
    {
        var result = await _service.GetByIdWithCommentsAsync(bookId);
        if (result == null)
            return NotFound(ReponseDto.Create(HttpStatusCode.NotFound, "Livro não encontrado."));

        return Ok(ReponseDto.Create(HttpStatusCode.OK, "Informações carregadas com sucesso.", result));
    }
}
