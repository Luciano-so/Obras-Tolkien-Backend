namespace TerraMedia.Application.Dtos;

public class BookCommentDto
{
    public Guid? Id { get; set; }
    public Guid? BookId { get; set; }
    public Guid UserId { get; set; }
    public string Comment { get; set; } = null!;
}
