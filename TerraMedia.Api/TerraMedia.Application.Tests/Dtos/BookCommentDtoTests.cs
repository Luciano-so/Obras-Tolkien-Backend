using TerraMedia.Application.Dtos;

namespace TerraMedia.Application.Tests.Dtos;

public class BookCommentDtoTests
{
    [Fact]
    public void Create_BookCommentDto_ShouldSetProperties()
    {
        var id = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddHours(-1);
        var updatedAt = DateTime.UtcNow;
        var userName = "John Doe";
        var commentText = "This is a comment.";
        var isNew = true;

        var commentDto = new BookCommentDto
        {
            Id = id,
            BookId = bookId,
            UserId = userId,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            User = userName,
            Comment = commentText,
            IsNew = isNew
        };

        Assert.Equal(id, commentDto.Id);
        Assert.Equal(bookId, commentDto.BookId);
        Assert.Equal(userId, commentDto.UserId);
        Assert.Equal(createdAt, commentDto.CreatedAt);
        Assert.Equal(updatedAt, commentDto.UpdatedAt);
        Assert.Equal(userName, commentDto.User);
        Assert.Equal(commentText, commentDto.Comment);
        Assert.True(commentDto.IsNew);
    }
}
