using TerraMedia.Application.Dtos;

namespace TerraMedia.Application.Tests.Dtos;

public class CreateUserDtoTests
{
    [Fact]
    public void Create_CreateUserDto_ShouldSetProperties()
    {
        var id = Guid.NewGuid();

        var commentDto = new CreateUserDto { Login = "login", Name = "nome", Password = "Teste" };

        Assert.Equal("login", commentDto.Login);
        Assert.Equal("nome", commentDto.Name);
        Assert.Equal("Teste", commentDto.Password);
    }
}
