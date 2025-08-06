using TerraMedia.Domain.Contracts.Data;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Domain.Contracts.IRepositories;

public interface IBookCommentRepository : IRepository
{
    Task MarkCommentAsAddedAsync(BookComment comment);
    Task<BookComment?> GetByCommentsAsync(Guid bookId, Guid commentId);
}