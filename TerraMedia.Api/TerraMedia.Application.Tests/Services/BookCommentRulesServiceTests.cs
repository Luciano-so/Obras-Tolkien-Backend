using TerraMedia.Application.Exceptions;
using TerraMedia.Application.Services;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Application.Tests.Services;

public class BookCommentRulesServiceTests
{
    private readonly BookCommentRulesService _service;

    public BookCommentRulesServiceTests()
    {
        _service = new BookCommentRulesService();
    }
    
    [Fact]
    public async Task IsAllowedToCommentAsync_SingleComment_Allowed()
    {
        var userId = Guid.NewGuid();
        var book = Book.Factory.Create(1);
        var service = new BookCommentRulesService();

        book.AddComment(BookComment.Factory.Create(userId, book, "Primeiro comentário"));

        await service.IsAllowedToCommentAsync(userId, book);
    }

    [Fact]
    public async Task IsAllowedToCommentAsync_BookNotFound_ThrowsBusinessException()
    {
        Book book = null;

        var exception = await Assert.ThrowsAsync<BusinessException>(() => _service.IsAllowedToCommentAsync(Guid.NewGuid(), book));
        Assert.Equal("Livro não encontrado.", exception.Message);
    }

    [Fact]
    public async Task IsAllowedToCommentAsync_LimitReachedPerMinute_ThrowsBusinessException()
    {
        var userId = Guid.NewGuid();
        var book = Book.Factory.Create(1);

        book.AddComment(BookComment.Factory.Create(userId, book, "Comentário 1"));
        book.AddComment(BookComment.Factory.Create(userId, book, "Comentário 2"));
        book.AddComment(BookComment.Factory.Create(userId, book, "Comentário 3"));

        var service = new BookCommentRulesService();

        var exception = await Assert.ThrowsAsync<BusinessException>(() => service.IsAllowedToCommentAsync(userId, book));
        Assert.Equal("Limite de 3 comentários por minuto atingido.", exception.Message);
    }

    [Fact]
    public async Task IsAllowedToCommentAsync_LimitReached_PerBook_ThrowsBusinessException()
    {
        var userId = Guid.NewGuid();
        var book = Book.Factory.Create(1);
        var service = new BookCommentRulesService();

        var totalComments = 11;
        var commentsPerCycle = 3;
        int commentCount = 0;

        for (int i = 0; i < totalComments - 1; i++)
        {
            book.AddComment(BookComment.Factory.Create(userId, book, $"Comentário {i + 1}"));
            commentCount++;

            if (commentCount % commentsPerCycle == 0)
            {
                await Task.Delay(60000);
            }
        }

        var exception = await Assert.ThrowsAsync<BusinessException>(() => service.IsAllowedToCommentAsync(userId, book));
        Assert.Equal("Limite de 10 comentários por livro atingido. Edite ou exclua um comentário existente.", exception.Message);
    }
}
