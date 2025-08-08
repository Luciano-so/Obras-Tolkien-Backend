using TerraMedia.Application.Dtos;

namespace TerraMedia.Application.Tests.Dtos;

public class UserDtoTests
{
    [Fact]
    public void Create_UsertDto_ShouldSetProperties()
    {
        var userName = "John Doe";

        var commentDto = new UserDto { Name = userName, Active = true, Login = "Teste", CreatedAt = DateTime.Now, Id = Guid.NewGuid() };

        Assert.Equal(userName, commentDto.Name);
    }
}
