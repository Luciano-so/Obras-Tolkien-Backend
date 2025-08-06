using TerraMedia.Application.Exceptions;
using TerraMedia.Application.Interfaces;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Application.Services;

public class BookCommentRulesService : IBookCommentRulesService
{
    public BookCommentRulesService() { }

    public Task IsAllowedToCommentAsync(Guid userId, Book book)
    {
        if (book is null)
            throw new BusinessException("Livro não encontrado.");

        ValidateCommentsPerMinute(userId, book);
        ValidateCommentsPerBook(userId, book);

        return Task.CompletedTask;
    }

    private void ValidateCommentsPerMinute(Guid userId, Book book)
    {
        var now = DateTime.UtcNow;
        var recentCommentsCount = book.Comments
            .Count(c => c.UserId == userId && (now - c.CreatedAt).TotalSeconds <= 60);

        if (recentCommentsCount >= 3)
            throw new BusinessException("Limite de 3 comentários por minuto atingido.");
    }

    private void ValidateCommentsPerBook(Guid userId, Book book)
    {
        var totalCommentsByUser = book.Comments.Count(c => c.UserId == userId);

        if (totalCommentsByUser >= 10)
            throw new BusinessException("Limite de 10 comentários por livro atingido. Edite ou exclua um comentário existente.");
    }
}
