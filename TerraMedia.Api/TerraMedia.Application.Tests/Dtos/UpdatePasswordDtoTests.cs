using TerraMedia.Application.Dtos;

namespace TerraMedia.Application.Tests.Dtos;

public class UpdatePasswordDtoTests
{
    [Fact]
    public void Create_UpdatePasswordDto_ShouldSetProperties()
    {
        var id = Guid.NewGuid();

        var commentDto = new UpdatePasswordDto { Id = id, Password = "Teste" };

        Assert.Equal(id, commentDto.Id);
        Assert.Equal("Teste", commentDto.Password);
    }
}
