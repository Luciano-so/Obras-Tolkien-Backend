using Microsoft.Extensions.Options;
using Moq;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Services;
using TerraMedia.Domain.Contracts.IRepositories;
using TerraMedia.Domain.Entities;
using TerraMedia.Domain.Settings;

namespace TerraMedia.Application.Tests.Services;

public class AuthenticateServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IOptions<AuthorizeSettings>> _mockOptions;
    private readonly AuthenticateService _authenticateService;

    public AuthenticateServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockOptions = new Mock<IOptions<AuthorizeSettings>>();
        _mockOptions.Setup(o => o.Value).Returns(new AuthorizeSettings { Secret = "mySecret", Expires = 60 });

        _authenticateService = new AuthenticateService(_mockUserRepository.Object, _mockOptions.Object);
    }

    [Fact]
    public async Task AuthenticateAsync_InvalidCredentials_ThrowsUnauthorizedAccessException()
    {
        var authenticateDto = new AuthenticateDto
        {
            Login = "Teste",
            Password = "invalidPassword"
        };

        _mockUserRepository.Setup(r => r.Authenticate(authenticateDto.Login, It.IsAny<string>())).ReturnsAsync((User)null);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authenticateService.AuthenticateAsync(authenticateDto));
        Assert.Equal("Usuário ou senha inválido.", exception.Message);
    }
}
