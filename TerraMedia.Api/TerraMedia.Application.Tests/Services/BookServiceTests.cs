using AutoMapper;
using Moq;
using TerraMedia.Application.Dtos;
using TerraMedia.Application.Interfaces;
using TerraMedia.Application.Services;
using TerraMedia.Domain.Contracts.Data;
using TerraMedia.Domain.Contracts.IRepositories;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Application.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _mockRepository;
    private readonly Mock<IBookCommentRulesService> _mockRulesService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _mockRepository = new Mock<IBookRepository>();
        _mockRulesService = new Mock<IBookCommentRulesService>();
        _mockMapper = new Mock<IMapper>();
        _bookService = new BookService(_mockRepository.Object, _mockMapper.Object, _mockRulesService.Object);
    }

    [Fact]
    public async Task CreateBookWithCommentAsync_ValidData_CreatesBookAndComment()
    {
        var coverId = 1;
        var userId = Guid.NewGuid();
        var commentDto = new BookCommentDto { Comment = "Great Book!" };
        var book = Book.Factory.Create(coverId);
        var comment = BookComment.Factory.Create(userId, book, commentDto.Comment);
        var bookDto = new BookDto
        {
            Id = book.Id,
            Comments = new List<BookCommentDto>()
        };

        bookDto.Comments.Add(new BookCommentDto { Comment = commentDto.Comment });


        _mockRepository.Setup(r => r.GetBook(coverId)).ReturnsAsync((Book)null);
        _mockMapper.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(bookDto);
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _mockRepository.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);
        var result = await _bookService.CreateBookWithCommentAsync(coverId, userId, commentDto);

        Assert.NotNull(result);
        Assert.Equal(bookDto.Comments.First().Comment, result.Comments.First().Comment);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
        _mockRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task AddCommentToExistingBookAsync_ValidData_AddsComment()
    {
        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var commentText = "This is an excellent book!";
        var commentDto = new CommentDto { Comment = commentText };

        var book = Book.Factory.Create(1);

        typeof(Book)
            .GetProperty(nameof(Book.Id))
            .SetValue(book, bookId);

        var initialComment = BookComment.Factory.Create(userId, book, "First comment");
        book.AddComment(initialComment);

        _mockRepository.Setup(r => r.GetByIdWithCommentsAsync(bookId)).ReturnsAsync(book);
        _mockRulesService.Setup(r => r.IsAllowedToCommentAsync(userId, book)).Returns(Task.CompletedTask);
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _mockRepository.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        _mockMapper.Setup(m => m.Map<BookDto>(It.IsAny<Book>()))
            .Returns((Book b) => new BookDto
            {
                Id = b.Id,
                Comments = b.Comments.Select(c => new BookCommentDto { Comment = c.Comment }).ToList()
            });

        var result = await _bookService.AddCommentToExistingBookAsync(bookId, userId, commentDto);

        Assert.NotNull(result);
        Assert.Contains(result.Comments, c => c.Comment == commentText);
        _mockRepository.Verify(r => r.MarkCommentAsAddedAsync(It.Is<BookComment>(c => c.Comment == commentText)), Times.Once);
        _mockRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task UpdateCommentAsync_ValidData_UpdatesComment()
    {

        var userId = Guid.NewGuid();
        var commentDto = new CommentDto { Comment = "Updated comment!" };
        var book = Book.Factory.Create(1);

        var existingComment = BookComment.Factory.Create(userId, book, "Initial comment");
        book.AddComment(existingComment);

        _mockRepository.Setup(r => r.GetByIdWithCommentsAsync(book.Id)).ReturnsAsync(book);
        _mockMapper.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(new BookDto { Id = book.Id });

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _mockRepository.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        var result = await _bookService.UpdateCommentAsync(book.Id, userId, existingComment.Id, commentDto);

        Assert.NotNull(result);
        _mockRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }


    [Fact]
    public async Task CreateCommentAsync_ValidData_UpdatesComment()
    {

        var userId = Guid.NewGuid();
        var commentDto = new CommentDto { Comment = "Updated comment!" };
        var book = Book.Factory.Create(1);

        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddHours(-1);
        var updatedAt = DateTime.UtcNow;
        var userName = "John Doe";
        var commentText = "This is a comment.";
        var isNew = true;

        var bookCommentDto = new BookCommentDto
        {
            Id = id,
            BookId = book.Id,
            UserId = userId,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            User = userName,
            Comment = commentText,
            IsNew = isNew
        };


        var existingComment = BookComment.Factory.Create(userId, book, "Initial comment");
        book.AddComment(existingComment);

        _mockRepository.Setup(r => r.GetByIdWithCommentsAsync(book.Id)).ReturnsAsync(book);
        _mockRepository.Setup(r => r.GetBook(book.CoverId)).ReturnsAsync(book);
        _mockMapper.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(new BookDto { Id = book.Id });
        _mockMapper.Setup(m => m.Map<CommentDto>(It.IsAny<BookCommentDto>()))
                        .Returns((BookCommentDto dto) => new CommentDto
                        {
                            Comment = dto.Comment
                        });

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _mockRepository.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);

        var result = await _bookService.CreateBookWithCommentAsync(1, userId, bookCommentDto);

        Assert.NotNull(result);
        _mockRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }


    [Fact]
    public async Task RemoveCommentAsync_ValidData_RemovesComment()
    {
        var userId = Guid.NewGuid();
        var commentDto = new CommentDto { Comment = "Updated comment!" };
        var book = Book.Factory.Create(1);

        var existingComment = BookComment.Factory.Create(userId, book, "Initial comment");
        book.AddComment(existingComment);

        _mockRepository.Setup(r => r.GetByIdWithCommentsAsync(book.Id)).ReturnsAsync(book);
        _mockMapper.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(new BookDto { Id = book.Id });

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);
        _mockRepository.Setup(r => r.UnitOfWork).Returns(unitOfWorkMock.Object);
        var result = await _bookService.RemoveCommentAsync(book.Id, existingComment.Id);

        Assert.NotNull(result);
        Assert.DoesNotContain(result.Comments, c => c.Id == existingComment.Id);
        _mockRepository.Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact]
    public async Task GetByIdWithCommentsAsync_ValidData_ReturnsBookWithComments()
    {
        var coverId = 1;
        var book = Book.Factory.Create(coverId);
        book.AddComment(BookComment.Factory.Create(Guid.NewGuid(), book, "First comment"));

        var bookDto = new BookDto
        {
            Id = book.Id,
            Comments = book.Comments.Select(c => new BookCommentDto { Comment = c.Comment }).ToList()
        };

        _mockRepository.Setup(r => r.GetBook(coverId)).ReturnsAsync(book);
        _mockMapper.Setup(m => m.Map<BookDto>(book)).Returns(bookDto);
        var result = await _bookService.GetByIdWithCommentsAsync(coverId);

        Assert.NotNull(result);
        Assert.Equal(book.Comments.Count, result.Comments.Count);
        _mockRepository.Verify(r => r.GetBook(coverId), Times.Once);
    }
}
