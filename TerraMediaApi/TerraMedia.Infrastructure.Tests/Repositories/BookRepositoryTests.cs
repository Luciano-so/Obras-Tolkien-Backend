using Microsoft.EntityFrameworkCore;
using TerraMedia.Domain.Entities;
using TerraMedia.Infrastructure.Persistence.Repositories;

namespace TerraMedia.Infrastructure.Tests.Repositories;

public class BookRepositoryTests : IClassFixture<DataBaseTestFixture>
{
    private readonly DataBaseTestFixture _fixture;
    private readonly BookRepository _repository;

    public BookRepositoryTests(DataBaseTestFixture fixture)
    {
        _fixture = fixture;
        _repository = new BookRepository(_fixture.Context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddBook()
    {
        var book = Book.Factory.Create(1122);

        await _repository.AddAsync(book);
        await _fixture.Context.SaveChangesAsync();

        var savedBook = await _fixture.Context.Books.FindAsync(book.Id);
        Assert.NotNull(savedBook);
        Assert.Equal(1122, savedBook.CoverId);
    }

    [Fact]
    public async Task GetBook_ShouldReturnBookWithCommentsAndUsers()
    {
        var user = User.Factory.Create("Nome", "login", "senha123");
        await _fixture.Context.Users.AddAsync(user);

        var book = Book.Factory.Create(123);
        await _repository.AddAsync(book);

        var comment = BookComment.Factory.Create(user.Id, book, "Comentário de teste");
        book.AddComment(comment);
        await _fixture.Context.SaveChangesAsync();

        var result = await _repository.GetBook(123);

        Assert.NotNull(result);
        Assert.Equal(123, result.CoverId);
        Assert.NotEmpty(result.Comments);
        Assert.Contains(result.Comments, c => c.UserId == user.Id);
    }

    [Fact]
    public async Task GetByIdWithCommentsAsync_ShouldReturnBookWithComments()
    {
        var book = Book.Factory.Create(555);
        await _repository.AddAsync(book);

        var comment = BookComment.Factory.Create(Guid.NewGuid(), book, "Comentário teste");
        book.AddComment(comment);
        await _fixture.Context.SaveChangesAsync();

        var result = await _repository.GetByIdWithCommentsAsync(book.Id);

        Assert.NotNull(result);
        Assert.Equal(book.Id, result.Id);
        Assert.NotEmpty(result.Comments);
    }

    [Fact]
    public async Task MarkCommentAsAddedAsync_ShouldSetEntityStateAdded()
    {
        var book = Book.Factory.Create(1);
        await _repository.AddAsync(book);

        var user = User.Factory.Create("Nome", "login", "senha123");
        await _fixture.Context.Users.AddAsync(user);
        await _fixture.Context.SaveChangesAsync();

        var comment = BookComment.Factory.Create(user.Id, book, "Comentário");

        var entryBefore = _fixture.Context.Entry(comment);
        Assert.Equal(EntityState.Detached, entryBefore.State);

        await _repository.MarkCommentAsAddedAsync(comment);

        var entryAfter = _fixture.Context.Entry(comment);
        Assert.Equal(EntityState.Added, entryAfter.State);
    }
}
