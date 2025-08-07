using Microsoft.EntityFrameworkCore;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Infrastructure.Tests.Contexts;

public class MainContextTests : IClassFixture<DataBaseTestFixture>
{
    private readonly DataBaseTestFixture _fixture;

    public MainContextTests(DataBaseTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddUser_Commit_ShouldPersistUser()
    {
        var context = _fixture.Context;

        context.ChangeTracker.Clear();

        var user = User.Factory.Create("Test User", "testlogin", "password123");
        await context.Users.AddAsync(user);

        var result = await context.Commit();

        Assert.True(result);
        Assert.Equal(1, await context.Users.CountAsync());
        Assert.NotEqual(Guid.Empty, user.Id);
    }

    [Fact]
    public async Task Commit_WhenNoChanges_ShouldReturnFalse()
    {
        var context = _fixture.Context;

        context.ChangeTracker.Clear();

        var result = await context.Commit();

        Assert.False(result);
    }
}
