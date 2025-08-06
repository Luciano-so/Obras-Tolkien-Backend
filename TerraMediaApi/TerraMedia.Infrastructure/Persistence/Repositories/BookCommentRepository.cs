using Microsoft.EntityFrameworkCore;
using TerraMedia.Domain.Contracts.Data;
using TerraMedia.Domain.Contracts.IRepositories;
using TerraMedia.Domain.Entities;
using TerraMedia.Infrastructure.Persistence.Contexts;

namespace TerraMedia.Infrastructure.Persistence.Repositories;

public class BookCommentRepository : IBookCommentRepository
{
    private readonly MainContext _context;

    public IUnitOfWork UnitOfWork => _context;
    public BookCommentRepository(MainContext context) => _context = context;

    public async Task<BookComment?> GetByCommentsAsync(Guid bookId, Guid commentId)
    {
        return await _context.BookComments
                             .Where(t => t.BookId == bookId && t.Id == commentId)
                             .FirstOrDefaultAsync();
    }

    public Task MarkCommentAsAddedAsync(BookComment comment)
    {
        _context.Entry(comment).State = EntityState.Added;
        return Task.CompletedTask;
    }
}
