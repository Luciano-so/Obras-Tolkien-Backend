namespace TerraMedia.Application.Dtos;

public class UpdatePasswordDto
{
    public Guid Id { get; set; }
    public string Password { get; set; } = string.Empty;
}
