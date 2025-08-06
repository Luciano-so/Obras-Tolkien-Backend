using AutoMapper;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Interfaces;
using TerraMedia.Domain.Contracts.IRepositories;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Application.Services;

public class BookService : IBookService
{
    private readonly IMapper _mapper;
    private readonly IBookRepository _repository;
    private readonly IBookCommentRulesService _rulesService;

    public BookService(IBookRepository repository, IMapper mapper, IBookCommentRulesService rulesService)
    {
        _mapper = mapper;
        _repository = repository;
        _rulesService = rulesService;
    }

    public async Task<BookDto> CreateBookWithCommentAsync(int coverId, Guid userId, BookCommentDto commentDto)
    {
        var existingBook = await _repository.GetBook(coverId);
        if (existingBook != null)
        {
            var mappedCommentDto = _mapper.Map<CommentDto>(commentDto);
            return await AddCommentToExistingBookAsync(existingBook.Id, userId, mappedCommentDto);
        }

        var book = Book.Factory.Create(coverId);
        var comment = BookComment.Factory.Create(userId, book, commentDto.Comment);
        book.AddComment(comment);

        await _repository.AddAsync(book);
        await _repository.UnitOfWork.Commit();

        return await GetBookDtoByIdAsync(book.Id);
    }

    public async Task<BookDto> AddCommentToExistingBookAsync(Guid bookId, Guid userId, CommentDto commentDto)
    {
        var book = await GetBookOrThrowAsync(bookId);
        await _rulesService.IsAllowedToCommentAsync(userId, book);

        var newComment = BookComment.Factory.Create(userId, book, commentDto.Comment);
        book.AddComment(newComment);

        await _repository.MarkCommentAsAddedAsync(newComment);
        await _repository.UnitOfWork.Commit();

        return await GetBookDtoByIdAsync(bookId);
    }

    public async Task<BookDto> UpdateCommentAsync(Guid bookId, Guid userId, Guid commentId, CommentDto commentDto)
    {
        var book = await GetBookOrThrowAsync(bookId);

        var existingComment = book.Comments.FirstOrDefault(c => c.Id == commentId)
                              ?? throw new Exception("Comentário não encontrado.");

        existingComment.UpdateComment(commentDto.Comment);

        await _repository.UnitOfWork.Commit();

        var updatedBook = await _repository.GetByIdWithCommentsAsync(bookId);
        return await GetBookDtoByIdAsync(bookId);
    }

    public async Task<BookDto> RemoveCommentAsync(Guid bookId, Guid commentId)
    {
        var book = await GetBookOrThrowAsync(bookId);

        book.RemoveComment(commentId);

        await _repository.UnitOfWork.Commit();
        return await GetBookDtoByIdAsync(bookId);
    }

    public async Task<BookDto> GetByIdWithCommentsAsync(Guid bookId)
    {
        return await GetBookDtoByIdAsync(bookId);
    }

    private async Task<Book> GetBookOrThrowAsync(Guid bookId)
    {
        return await _repository.GetByIdWithCommentsAsync(bookId) ?? throw new InvalidOperationException("Livro não encontrado.");
    }

    private async Task<BookDto> GetBookDtoByIdAsync(Guid bookId)
    {
        var updatedBook = await _repository.GetByIdWithCommentsAsync(bookId);
        return _mapper.Map<BookDto>(updatedBook);
    }
}
