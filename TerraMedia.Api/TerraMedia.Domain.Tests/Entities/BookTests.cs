using TerraMedia.Domain.Entities;
using TerraMedia.Domain.Exceptions;

namespace TerraMedia.Domain.Tests.Entities;

public class BookTests
{
    [Fact]
    public void Factory_Create_ValidCoverId_ShouldCreateBook()
    {
        var book = Book.Factory.Create(1);

        Assert.NotNull(book);
        Assert.Equal(1, book.CoverId);
        Assert.True(book.CreatedAt <= DateTime.UtcNow);
        Assert.Empty(book.Comments);
    }

    [Fact]
    public void AddComment_ValidComment_ShouldAddToComments()
    {
        var book = Book.Factory.Create(1);

        var userId = Guid.NewGuid();
        var comment = BookComment.Factory.Create(userId, book, "Coment�rio v�lido para teste");

        book.AddComment(comment);

        Assert.Single(book.Comments);
        Assert.Contains(comment, book.Comments);
    }

    [Fact]
    public void RemoveComment_ExistingComment_ShouldRemoveAndReturnTrue()
    {
        var book = Book.Factory.Create(1);

        var userId = Guid.NewGuid();
        var comment = BookComment.Factory.Create(userId, book, "Coment�rio para remover");

        book.AddComment(comment);

        var removed = book.RemoveComment(comment.Id);

        Assert.True(removed);
        Assert.Empty(book.Comments);
    }

    [Fact]
    public void RemoveComment_NonExistentComment_ShouldThrowException()
    {
        var book = Book.Factory.Create(1);
        var nonExistentId = Guid.NewGuid();

        var ex = Assert.Throws<InvalidOperationException>(() => book.RemoveComment(nonExistentId));
        Assert.Contains("Sequence contains no matching element", ex.Message);
    }
}