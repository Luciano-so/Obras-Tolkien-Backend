namespace TerraMedia.Application.Dtos;

public class BookCommentDto
{
    public Guid? Id { get; set; }
    public Guid? BookId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? User { get; set; }
    public string Comment { get; set; } = null!;
    public bool IsNew { get; set; } = false;
}
