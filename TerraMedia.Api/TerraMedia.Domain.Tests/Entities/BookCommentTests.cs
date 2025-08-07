using TerraMedia.Domain.Entities;
using TerraMedia.Domain.Exceptions;

namespace TerraMedia.Domain.Tests.Entities;

public class BookCommentTests
{
    private Book CreateValidBook() => Book.Factory.Create(1);

    [Fact]
    public void Factory_Create_ValidParameters_ShouldCreateBookComment()
    {
        var book = CreateValidBook();
        var userId = Guid.NewGuid();
        var commentText = "Comentário válido para teste";

        var comment = BookComment.Factory.Create(userId, book, commentText);

        Assert.NotNull(comment);
        Assert.Equal(userId, comment.UserId);
        Assert.Equal(book.Id, comment.BookId);
        Assert.Equal(commentText, comment.Comment);
        Assert.True(comment.CreatedAt <= DateTime.UtcNow);
        Assert.True(comment.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Factory_Create_InvalidUserId_ShouldThrowDomainException()
    {
        var book = CreateValidBook();
        var invalidUserId = Guid.Empty;

        var ex = Assert.Throws<DomainException>(() =>
            BookComment.Factory.Create(invalidUserId, book, "Comentário válido"));

        Assert.Contains("Usuário", ex.Message);
    }

    [Fact]
    public void Factory_Create_InvalidBook_ShouldThrowDomainException()
    {
        var userId = Guid.NewGuid();
        Book? nullBook = null;

        var ex = Assert.Throws<DomainException>(() =>
            BookComment.Factory.Create(userId, nullBook!, "Comentário válido"));
    }

    [Fact]
    public void Factory_Create_EmptyComment_ShouldThrowDomainException()
    {
        var book = CreateValidBook();
        var userId = Guid.NewGuid();

        var ex = Assert.Throws<DomainException>(() =>
            BookComment.Factory.Create(userId, book, ""));

        Assert.Contains("Comentário não pode estar vazio", ex.Message);
    }

    [Fact]
    public void Factory_Create_ShortComment_ShouldThrowDomainException()
    {
        var book = CreateValidBook();
        var userId = Guid.NewGuid();

        var ex = Assert.Throws<DomainException>(() =>
            BookComment.Factory.Create(userId, book, "abc"));

        Assert.Contains("comentário deve conter entre", ex.Message);
    }

    [Fact]
    public void UpdateComment_ValidComment_ShouldUpdateCommentAndUpdatedAt()
    {
        var book = CreateValidBook();
        var userId = Guid.NewGuid();

        var comment = BookComment.Factory.Create(userId, book, "Comentário inicial");

        var oldUpdatedAt = comment.UpdatedAt;

        comment.UpdateComment("Novo comentário atualizado");

        Assert.Equal("Novo comentário atualizado", comment.Comment);
        Assert.True(comment.UpdatedAt > oldUpdatedAt);
    }

    [Fact]
    public void UpdateComment_InvalidComment_ShouldThrowDomainException()
    {
        var book = CreateValidBook();
        var userId = Guid.NewGuid();

        var comment = BookComment.Factory.Create(userId, book, "Comentário válido");

        var ex = Assert.Throws<DomainException>(() => comment.UpdateComment("abc"));

        Assert.Contains("comentário deve conter entre", ex.Message);
    }
}
