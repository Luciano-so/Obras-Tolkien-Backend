using Microsoft.EntityFrameworkCore;
using TerraMedia.Domain.Contracts.Data;
using TerraMedia.Domain.Contracts.IRepositories;
using TerraMedia.Domain.Entities;
using TerraMedia.Infrastructure.Persistence.Contexts;

namespace TerraMedia.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly MainContext _context;

    public IUnitOfWork UnitOfWork => _context;
    public BookRepository(MainContext context) => _context = context;
    public async Task<Book?> GetBook(int coverId)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.CoverId == coverId);
    }
    public async Task<Book?> GetByIdWithCommentsAsync(Guid bookId)
    {
        return await _context.Books
                             .Where(b => b.Id == bookId)
                             .Include(b => b.Comments)
                             .FirstOrDefaultAsync();
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    public Task MarkCommentAsAddedAsync(BookComment comment)
    {
        _context.Entry(comment).State = EntityState.Added;
        return Task.CompletedTask;
    }
}
