using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using TerraMedia.Api.Controllers.V1;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Dtos.OpenLibrary;
using TerraMedia.Application.Interfaces;

namespace TerraMedia.Api.Tests.Controllers.V1;

public class OpenLibraryControllerTests
{
    private readonly Mock<IOpenLibraryService> _mockOpenLibraryService;
    private readonly OpenLibraryController _controller;

    public OpenLibraryControllerTests()
    {
        _mockOpenLibraryService = new Mock<IOpenLibraryService>();
        _controller = new OpenLibraryController(_mockOpenLibraryService.Object);
    }

    [Fact]
    public async Task Search_ValidAuthor_ReturnsOkWithResults()
    {

        var author = "J.K. Rowling";
        var page = 1;
        var limit = 10;

        var expectedResult = new OpenLibrarySearchDto
        {
            Docs = new List<OpenLibraryBookDto>
        {
            new OpenLibraryBookDto { Title = "Harry Potter", Author_name = new List<string> { "J.K. Rowling" } }
        }
        };

        _mockOpenLibraryService
            .Setup(s => s.SearchBooksAsync(author, page, limit, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.Search(author, page, limit);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseDto = Assert.IsType<ResponseDto<OpenLibrarySearchDto>>(okResult.Value);

        Assert.Equal(HttpStatusCode.OK, responseDto.StatusCode);
        Assert.Equal("Busca realizada com sucesso", responseDto.Message);
        Assert.Equal(expectedResult, responseDto.Data);

        _mockOpenLibraryService.Verify(s => s.SearchBooksAsync(author, page, limit, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Search_InvalidAuthor_ReturnsBadRequest()
    {
        string author = null;

        var result = await _controller.Search(author, 1, 10);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var responseDto = Assert.IsType<ResponseDto>(badRequestResult.Value);

        Assert.Equal(HttpStatusCode.BadRequest, responseDto.StatusCode);
        Assert.Equal("É necessário informar o autor.", responseDto.Message);

        _mockOpenLibraryService.Verify(s => s.SearchBooksAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetAuthorBio_InvalidAuthorKey_ReturnsBadRequest()
    {
        string authorKey = null;

        var result = await _controller.GetAuthorBio(authorKey);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var responseDto = Assert.IsType<ResponseDto>(badRequestResult.Value);

        Assert.Equal(HttpStatusCode.BadRequest, responseDto.StatusCode);
        Assert.Equal("É necessário informar a chave do autor.", responseDto.Message);

        _mockOpenLibraryService.Verify(s => s.GetAuthorBioAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}