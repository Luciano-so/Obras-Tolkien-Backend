using Microsoft.EntityFrameworkCore;
using TerraMedia.Domain.Entities;
using TerraMedia.Infrastructure.Persistence.Repositories;

namespace TerraMedia.Infrastructure.Tests.Repositories;

public class BookCommentRepositoryTests : IClassFixture<DataBaseTestFixture>
{
    private readonly DataBaseTestFixture _fixture;
    private readonly BookCommentRepository _repository;

    public BookCommentRepositoryTests(DataBaseTestFixture fixture)
    {
        _fixture = fixture;
        _repository = new BookCommentRepository(_fixture.Context);
    }

    [Fact]
    public async Task GetByCommentsAsync_ShouldReturnComment_WhenExists()
    {
        var book = Book.Factory.Create(100);
        await _fixture.Context.Books.AddAsync(book);

        var user = User.Factory.Create("Nome", "login", "senha123");
        await _fixture.Context.Users.AddAsync(user);

        var comment = BookComment.Factory.Create(user.Id, book, "Comentário para teste");
        await _fixture.Context.BookComments.AddAsync(comment);
        await _fixture.Context.SaveChangesAsync();

        var result = await _repository.GetByCommentsAsync(book.Id, comment.Id);

        Assert.NotNull(result);
        Assert.Equal(comment.Id, result.Id);
        Assert.Equal(book.Id, result.BookId);
        Assert.Equal(user.Id, result.UserId);
    }

    [Fact]
    public async Task GetByCommentsAsync_ShouldReturnNull_WhenNotFound()
    {
        var result = await _repository.GetByCommentsAsync(Guid.NewGuid(), Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task MarkCommentAsAddedAsync_ShouldSetEntityStateAdded()
    {
        var book = Book.Factory.Create(9);
        await _fixture.Context.Books.AddAsync(book);

        var user = User.Factory.Create("Nome", "login", "senha123");
        await _fixture.Context.Users.AddAsync(user);
        await _fixture.Context.SaveChangesAsync();

        var comment = BookComment.Factory.Create(user.Id, book, "Comentário teste");

        var entryBefore = _fixture.Context.Entry(comment);
        Assert.Equal(EntityState.Detached, entryBefore.State);

        await _repository.MarkCommentAsAddedAsync(comment);

        var entryAfter = _fixture.Context.Entry(comment);
        Assert.Equal(EntityState.Added, entryAfter.State);
    }
}
