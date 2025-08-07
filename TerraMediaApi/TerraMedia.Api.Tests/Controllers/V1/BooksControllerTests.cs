using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Security.Claims;
using TerraMedia.Api.Controllers.V1;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Interfaces;

namespace TerraMedia.Api.Tests.Controllers.V1;

public class BooksControllerTests
{
    private readonly Mock<IBookService> _mockBookService;
    private readonly BooksController _controller;

    public BooksControllerTests()
    {
        _mockBookService = new Mock<IBookService>();
        _controller = new BooksController(_mockBookService.Object);
        var claims = new[] { new Claim("userId", Guid.NewGuid().ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        var user = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task CreateWithComment_ValidData_ReturnsCreatedResponse()
    {
        var coverId = 1;
        var commentDto = new BookCommentDto { Comment = "Great book!" };
        var bookDto = new BookDto
        {
            Id = Guid.NewGuid(),
            CoverId = coverId,
            Comments = new List<BookCommentDto> { commentDto }
        };

        _mockBookService
            .Setup(s => s.CreateBookWithCommentAsync(coverId, It.IsAny<Guid>(), commentDto))
            .ReturnsAsync(bookDto);

        var result = await _controller.CreateWithComment(coverId, commentDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseDto = Assert.IsType<ReponseDto<BookDto>>(okResult.Value);

        Assert.Equal(HttpStatusCode.Created, responseDto.StatusCode);
        Assert.Equal("Comentário cadastrado com sucesso", responseDto.Message);
        Assert.Equal(bookDto, responseDto.Data);

        _mockBookService.Verify(s => s.CreateBookWithCommentAsync(coverId, It.IsAny<Guid>(), commentDto), Times.Once);
    }

    [Fact]
    public async Task AddComment_ValidData_ReturnsOkResponse()
    {
        var bookId = Guid.NewGuid();
        var commentDto = new CommentDto { Comment = "Great book!" };
        var bookDto = new BookDto { Id = bookId, CoverId = 1 };

        _mockBookService
            .Setup(s => s.AddCommentToExistingBookAsync(bookId, It.IsAny<Guid>(), commentDto))
            .ReturnsAsync(bookDto);

        var result = await _controller.AddComment(bookId, commentDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseDto = Assert.IsType<ReponseDto<BookDto>>(okResult.Value);

        Assert.Equal(HttpStatusCode.OK, responseDto.StatusCode);
        Assert.Equal("Comentário cadastrado com sucesso", responseDto.Message);
        Assert.Equal(bookDto, responseDto.Data);

        _mockBookService.Verify(s => s.AddCommentToExistingBookAsync(bookId, It.IsAny<Guid>(), commentDto), Times.Once);
    }

    [Fact]
    public async Task UpdateComment_ValidData_ReturnsOkResponse()
    {
        var bookId = Guid.NewGuid();
        var commentId = Guid.NewGuid();
        var commentDto = new CommentDto { Comment = "Updated comment!" };
        var bookDto = new BookDto { Id = bookId, CoverId = 1 };

        _mockBookService
            .Setup(s => s.UpdateCommentAsync(bookId, It.IsAny<Guid>(), commentId, commentDto))
            .ReturnsAsync(bookDto);

        var result = await _controller.UpdateComment(bookId, commentId, commentDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseDto = Assert.IsType<ReponseDto<BookDto>>(okResult.Value);

        Assert.Equal(HttpStatusCode.OK, responseDto.StatusCode);
        Assert.Equal("Comentário atualizado com sucesso", responseDto.Message);
        Assert.Equal(bookDto, responseDto.Data);

        _mockBookService.Verify(s => s.UpdateCommentAsync(bookId, It.IsAny<Guid>(), commentId, commentDto), Times.Once);
    }

    [Fact]
    public async Task RemoveComment_ValidData_ReturnsNoContentResponse()
    {
        var bookId = Guid.NewGuid();
        var commentId = Guid.NewGuid();
        var expectedBookDto = new BookDto
        {
            Id = bookId,
            CoverId = 123,
            Comments = new List<BookCommentDto> { new BookCommentDto { Id = commentId, Comment = "Test comment" } }
        };

        _mockBookService
            .Setup(s => s.RemoveCommentAsync(bookId, commentId))
            .ReturnsAsync(expectedBookDto);

        var result = await _controller.RemoveComment(bookId, commentId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseDto = Assert.IsType<ReponseDto>(okResult.Value);

        Assert.Equal(HttpStatusCode.NoContent, responseDto.StatusCode);
        Assert.Equal("Comentário removido com sucesso", responseDto.Message);

        _mockBookService.Verify(s => s.RemoveCommentAsync(bookId, commentId), Times.Once);
    }

    [Fact]
    public async Task GetBook_BookExists_ReturnsOkResponse()
    {
        var coverId = 1;
        var bookDto = new BookDto { Id = Guid.NewGuid(), CoverId = coverId };

        _mockBookService
            .Setup(s => s.GetByIdWithCommentsAsync(coverId))
            .ReturnsAsync(bookDto);

        var result = await _controller.GetBook(coverId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseDto = Assert.IsType<ReponseDto<BookDto>>(okResult.Value);

        Assert.Equal(HttpStatusCode.OK, responseDto.StatusCode);
        Assert.Equal("Informações carregadas com sucesso.", responseDto.Message);
        Assert.Equal(bookDto, responseDto.Data);

        _mockBookService.Verify(s => s.GetByIdWithCommentsAsync(coverId), Times.Once);
    }

    [Fact]
    public async Task GetBook_BookNotFound_ReturnsNotFoundResponse()
    {
        var coverId = 1;

        _mockBookService
            .Setup(s => s.GetByIdWithCommentsAsync(coverId))
            .ReturnsAsync((BookDto)null);

        var result = await _controller.GetBook(coverId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var responseDto = Assert.IsType<ReponseDto>(notFoundResult.Value);

        Assert.Equal(HttpStatusCode.NotFound, responseDto.StatusCode);
        Assert.Equal("Livro não encontrado.", responseDto.Message);

        _mockBookService.Verify(s => s.GetByIdWithCommentsAsync(coverId), Times.Once);
    }
}
