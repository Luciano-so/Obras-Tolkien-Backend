using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using TerraMedia.Api.Controllers.V1;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Interfaces;


namespace TerraMedia.Api.Tests.Controllers.V1;
public class AuthenticateControllerTests
{
    private readonly Mock<IAuthenticateService> _mockAuthService;
    private readonly AuthenticateController _controller;

    public AuthenticateControllerTests()
    {
        _mockAuthService = new Mock<IAuthenticateService>();
        _controller = new AuthenticateController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Authenticate_ValidCredentials_ReturnsOkWithToken()
    {
        var authDto = new AuthenticateDto
        {
            Login = "user",
            Password = "password"
        };

        var expectedTokenDto = new TokenDto { AccessToken = "token123" };

        _mockAuthService
            .Setup(s => s.AuthenticateAsync(authDto))
            .ReturnsAsync(expectedTokenDto);

        var actionResult = await _controller.Authenticate(authDto);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);

        var responseDto = Assert.IsType<ReponseDto<TokenDto>>(okResult.Value);

        Assert.Equal(HttpStatusCode.OK, responseDto.StatusCode);
        Assert.Equal("Usuario autenticado com sucesso", responseDto.Message);
        Assert.Equal(expectedTokenDto, responseDto.Data);

        _mockAuthService.Verify(s => s.AuthenticateAsync(authDto), Times.Once);
    }    
}
