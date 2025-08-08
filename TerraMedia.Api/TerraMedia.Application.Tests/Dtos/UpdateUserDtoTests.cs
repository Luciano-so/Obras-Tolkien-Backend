using TerraMedia.Application.Dtos;

namespace TerraMedia.Application.Tests.Dtos;

public class UpdateUserDtoTests
{
    [Fact]
    public void Create_UpdateUserDto_ShouldSetProperties()
    {
        var id = Guid.NewGuid();

        var commentDto = new UpdateUserDto { Id = id, Status = true };

        Assert.Equal(id, commentDto.Id);
        Assert.True(commentDto.Status);
    }
}
