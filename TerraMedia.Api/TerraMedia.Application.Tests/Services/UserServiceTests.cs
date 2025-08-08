using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Exceptions;
using TerraMedia.Application.Services;
using TerraMedia.Domain.Contracts.Data;
using TerraMedia.Domain.Contracts.IRepositories;
using TerraMedia.Domain.Entities;
using TerraMedia.Domain.Settings;

namespace TerraMedia.Application.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IOptions<AuthorizeSettings>> _mockOptions;
    private readonly Mock<IMapper> _mapper;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockOptions = new Mock<IOptions<AuthorizeSettings>>();
        _mapper = new Mock<IMapper>();
        _mockOptions.Setup(o => o.Value).Returns(new AuthorizeSettings { Secret = "mySecret", Expires = 60 });

        _userService = new UserService(_mockUserRepository.Object, _mockOptions.Object, _mapper.Object);
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

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.AuthenticateAsync(authenticateDto));
        Assert.Equal("Usuário ou senha inválido.", exception.Message);
    }

    [Fact]
    public async Task CreateUserAsync_LoginAlreadyExists_ThrowsAppException()
    {
        var dto = new CreateUserDto { Login = "existingUser" };
        _mockUserRepository.Setup(r => r.ExistsUser(dto.Login)).ReturnsAsync(true);

        var ex = await Assert.ThrowsAsync<AppException>(() => _userService.CreateUserAsync(dto));
        Assert.Equal("Já existe um usuário com este login.", ex.Message);
    }

    [Fact]
    public async Task CreateUserAsync_ValidDto_AddsUserAndCommits()
    {
        var dto = new CreateUserDto { Login = "newUser", Name = "User", Password = "123456" };
        var user = User.Factory.Create(dto.Name, dto.Login, dto.Password);

        _mockUserRepository.Setup(r => r.ExistsUser(dto.Login)).ReturnsAsync(false);
        _mapper.Setup(m => m.Map<User>(dto)).Returns(user);
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _mockUserRepository.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);
        await _userService.CreateUserAsync(dto);

        _mockUserRepository.Verify(r => r.AddAsync(user), Times.Once);
        _mockUserRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task UpdatePasswordAsync_InvalidUser_ThrowsAppException()
    {
        var dto = new UpdatePasswordDto { Id = Guid.NewGuid(), Password = "123456" };
        _mockUserRepository.Setup(r => r.FindAsync(dto.Id)).ReturnsAsync((User)null);

        var ex = await Assert.ThrowsAsync<AppException>(() => _userService.UpdatePasswordAsync(dto));
        Assert.Equal("Usuário não encontrado.", ex.Message);
    }

    [Fact]
    public async Task UpdatePasswordAsync_ValidUser_UpdatesPassword()
    {
        var user = User.Factory.Create("Teste123", "Teste123", "123456");
        var dto = new UpdatePasswordDto { Id = user.Id, Password = "123456" };

        _mockUserRepository.Setup(r => r.FindAsync(dto.Id)).ReturnsAsync(user);
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _mockUserRepository.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);
        await _userService.UpdatePasswordAsync(dto);

        _mockUserRepository.Verify(r => r.Update(user), Times.Once);
        _mockUserRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_InvalidUser_ThrowsAppException()
    {
        var dto = new UpdateUserDto { Id = Guid.NewGuid(), Status = true };
        _mockUserRepository.Setup(r => r.FindAsync(dto.Id)).ReturnsAsync((User)null);

        var ex = await Assert.ThrowsAsync<AppException>(() => _userService.UpdateUserAsync(dto));
        Assert.Equal("Usuário não encontrado.", ex.Message);
    }

    [Fact]
    public async Task UpdateUserAsync_ValidUser_UpdatesStatus()
    {
        var user = User.Factory.Create("Teste123", "Teste123", "123456");
        var dto = new UpdateUserDto { Id = user.Id, Status = true };


        _mockUserRepository.Setup(r => r.FindAsync(dto.Id)).ReturnsAsync(user);
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _mockUserRepository.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        await _userService.UpdateUserAsync(dto);

        _mockUserRepository.Verify(r => r.Update(user), Times.Once);
        _mockUserRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsMappedUsers()
    {
        var user = User.Factory.Create("Teste123", "Teste123", "123456");
        var users = new List<User> { user };
        var userDtos = new List<UserDto> { new UserDto(), new UserDto() };

        _mockUserRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
        _mapper.Setup(m => m.Map<List<UserDto>>(users)).Returns(userDtos);

        var result = await _userService.GetAllUsersAsync();

        Assert.Equal(userDtos.Count, result.Count);
    }

}
