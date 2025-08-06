namespace TerraMedia.Application.Dtos;

public class BookDto
{
    public Guid Id { get; set; }
    public int CoverId { get; set; }
    public List<BookCommentDto> Comments { get; set; } = [];
}
