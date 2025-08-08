using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using TerraMedia.Api.Controllers.V1;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Interfaces;


namespace TerraMedia.Api.Tests.Controllers.V1;
public class AuthenticateControllerTests
{
    private readonly Mock<IUserService> _mockAuthService;
    private readonly AuthenticateController _controller;

    public AuthenticateControllerTests()
    {
        _mockAuthService = new Mock<IUserService>();
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

        var responseDto = Assert.IsType<ResponseDto<TokenDto>>(okResult.Value);

        Assert.Equal(HttpStatusCode.OK, responseDto.StatusCode);
        Assert.Equal("Usuario autenticado com sucesso", responseDto.Message);
        Assert.Equal(expectedTokenDto, responseDto.Data);

        _mockAuthService.Verify(s => s.AuthenticateAsync(authDto), Times.Once);
    }
    [Fact]
    public async Task Create_ValidUser_ReturnsCreated()
    {
        var createUserDto = new CreateUserDto
        {
            Name = "John Doe",
            Login = "john.doe",
            Password = "securepass"
        };

        _mockAuthService
            .Setup(s => s.CreateUserAsync(createUserDto))
            .Returns(Task.CompletedTask);

        var result = await _controller.Create(createUserDto);

        var createdResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsAssignableFrom<ResponseDto>(createdResult.Value);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("Usuário criado com sucesso", response.Message);
        Assert.Null(response.Data);

        _mockAuthService.Verify(s => s.CreateUserAsync(createUserDto), Times.Once);
    }

    [Fact]
    public async Task UpdatePassword_ValidDto_ReturnsOk()
    {
        var updatePasswordDto = new UpdatePasswordDto
        {
            Id = Guid.NewGuid(),
            Password = "newpass"
        };

        _mockAuthService
            .Setup(s => s.UpdatePasswordAsync(updatePasswordDto))
            .Returns(Task.CompletedTask);

        var result = await _controller.UpdatePassword(updatePasswordDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsAssignableFrom<ResponseDto>(okResult.Value);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Senha atualizada com sucesso", response.Message);
        Assert.Null(response.Data);

        _mockAuthService.Verify(s => s.UpdatePasswordAsync(updatePasswordDto), Times.Once);
    }

    [Fact]
    public async Task UpdateStatus_ValidDto_ReturnsOk()
    {
        var updateUserDto = new UpdateUserDto
        {
            Id = Guid.NewGuid(),
            Status = true
        };

        _mockAuthService
            .Setup(s => s.UpdateUserAsync(updateUserDto))
            .Returns(Task.CompletedTask);

        var result = await _controller.UpdateStatus(updateUserDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsAssignableFrom<ResponseDto>(okResult.Value);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Status do usuário atualizado com sucesso", response.Message);
        Assert.Null(response.Data);

        _mockAuthService.Verify(s => s.UpdateUserAsync(updateUserDto), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsListOfUsers()
    {
        var userList = new List<UserDto>
    {
        new UserDto { Name = "John", Login = "john" },
        new UserDto { Name = "Jane", Login = "jane" }
    };

        _mockAuthService
            .Setup(s => s.GetAllUsersAsync())
            .ReturnsAsync(userList);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseDto<List<UserDto>>>(okResult.Value);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Usuários retornados com sucesso", response.Message);
        Assert.Equal(userList, response.Data);

        _mockAuthService.Verify(s => s.GetAllUsersAsync(), Times.Once);
    }


}
