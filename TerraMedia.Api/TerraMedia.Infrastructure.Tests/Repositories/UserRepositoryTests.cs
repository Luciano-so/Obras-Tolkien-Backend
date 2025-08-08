using TerraMedia.Domain.Entities;
using TerraMedia.Infrastructure.Persistence.Repositories;

namespace TerraMedia.Infrastructure.Tests.Repositories;

public class UserRepositoryTests : IClassFixture<DataBaseTestFixture>
{
    private readonly DataBaseTestFixture _fixture;
    private readonly UserRepository _repository;

    public UserRepositoryTests(DataBaseTestFixture fixture)
    {
        _fixture = fixture;
        _repository = new UserRepository(_fixture.Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        var user = User.Factory.Create("Nome", "login", "senha123");

        await _repository.AddAsync(user);
        await _fixture.Context.SaveChangesAsync();

        var savedUser = await _fixture.Context.Users.FindAsync(user.Id);
        Assert.NotNull(savedUser);
        Assert.Equal(user.Login, savedUser.Login);
    }

    [Fact]
    public async Task Update_ShouldModifyUser()
    {
        var user = User.Factory.Create("Nome", "login", "senha123");
        await _repository.AddAsync(user);
        await _fixture.Context.SaveChangesAsync();

        user.Deactivate();
        _repository.Update(user);
        await _fixture.Context.SaveChangesAsync();

        var updatedUser = await _fixture.Context.Users.FindAsync(user.Id);
        Assert.False(updatedUser.Active);
    }

    [Fact]
    public async Task Remove_ShouldDeleteUser()
    {
        var user = User.Factory.Create("Nome", "login", "senha123");
        await _repository.AddAsync(user);
        await _fixture.Context.SaveChangesAsync();

        _repository.Remove(user);
        await _fixture.Context.SaveChangesAsync();

        var deletedUser = await _fixture.Context.Users.FindAsync(user.Id);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnUser_WhenCredentialsAreCorrect()
    {
        var user = User.Factory.Create("Nome", "login1", "senha123");
        await _repository.AddAsync(user);
        await _fixture.Context.SaveChangesAsync();
        var authenticatedUser = await _repository.Authenticate("login1", user.Password);

        Assert.NotNull(authenticatedUser);
        Assert.Equal(user.Id, authenticatedUser.Id);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnNull_WhenCredentialsAreIncorrect()
    {
        var user = User.Factory.Create("Nome", "login", "senha123");
        await _repository.AddAsync(user);
        await _fixture.Context.SaveChangesAsync();

        var authenticatedUser = await _repository.Authenticate("login", "senhaErrada");

        Assert.Null(authenticatedUser);
    }

    [Fact]
    public async Task Authenticate_ExistsUser()
    {
        var user = User.Factory.Create("Nome", "login", "senha123");
        await _repository.AddAsync(user);
        await _fixture.Context.SaveChangesAsync();

        var authenticatedUser = await _repository.ExistsUser("login");

        Assert.True(authenticatedUser);
    }

    [Fact]
    public async Task Authenticate_GetAllAsync()
    {
        var user = User.Factory.Create("Nome", "login", "senha123");
        await _repository.AddAsync(user);
        await _fixture.Context.SaveChangesAsync();

        var authenticatedUser = await _repository.GetAllAsync();

        Assert.True(authenticatedUser.Count > 0);
    }
}
