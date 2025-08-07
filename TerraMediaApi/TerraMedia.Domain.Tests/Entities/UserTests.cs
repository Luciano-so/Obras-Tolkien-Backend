using TerraMedia.Domain.Entities;
using TerraMedia.Domain.Exceptions;
using TerraMedia.Domain.Extensions;

namespace TerraMedia.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Factory_Create_ValidUser_ShouldCreateUser()
    {
        var user = User.Factory.Create("Luciano", "luciano", "123456");

        Assert.NotNull(user);
        Assert.True(user.Active);
        Assert.Equal("LUCIANO", user.Name);
        Assert.Equal("LUCIANO", user.Login);
        Assert.False(string.IsNullOrWhiteSpace(user.Password));
        Assert.True(user.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Factory_Create_EmptyName_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() =>
            User.Factory.Create("", "luciano", "123456"));
        Assert.Equal("O campo Nome do usuário não pode estar vazio.", ex.Message);
    }

    [Fact]
    public void Factory_Create_ShortName_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() =>
            User.Factory.Create("L", "luciano", "123456"));
        Assert.Equal("O nome do usuário deve conter entre 2 e 250 caracteres.", ex.Message);
    }

    [Fact]
    public void Factory_Create_EmptyLogin_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() =>
            User.Factory.Create("Luciano", "", "123456"));
        Assert.Equal("O campo login do usuário não pode estar vazio.", ex.Message);
    }

    [Fact]
    public void Factory_Create_ShortLogin_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() =>
            User.Factory.Create("Luciano", "L", "123456"));
        Assert.Equal("O login deve conter entre 2 e 50 caracteres.", ex.Message);
    }

    [Fact]
    public void Factory_Create_EmptyPassword_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() =>
            User.Factory.Create("Luciano", "luciano", ""));
        Assert.Equal("O campo Senha não pode estar vazio.", ex.Message);
    }

    [Fact]
    public void Factory_Create_ShortPassword_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() =>
            User.Factory.Create("Luciano", "luciano", "123"));
        Assert.Equal("A senha deve conter entre 6 e 100 caracteres.", ex.Message);
    }

    [Fact]
    public void Deactivate_ShouldSetActiveToFalse()
    {
        var user = User.Factory.Create("Luciano", "luciano", "123456");
        user.Deactivate();

        Assert.False(user.Active);
    }

    [Fact]
    public void Activate_ShouldSetActiveToTrue()
    {
        var user = User.Factory.Create("Luciano", "luciano", "123456");
        user.Deactivate();
        user.Activate();

        Assert.True(user.Active);
    }

    [Fact]
    public void ValidPassword_ValidNewPassword_ShouldUpdatePassword()
    {
        var user = User.Factory.Create("Luciano", "luciano", "123456");
        user.ValidPassword("novaSenha123");

        Assert.Equal("novaSenha123".Encrypt(), user.Password.Encrypt());
    }

    [Fact]
    public void ValidPassword_EmptyNewPassword_ShouldThrowDomainException()
    {
        var user = User.Factory.Create("Luciano", "luciano", "123456");

        var ex = Assert.Throws<DomainException>(() =>
            user.ValidPassword(""));
        Assert.Equal("O campo Senha não pode estar vazio.", ex.Message);
    }

    [Fact]
    public void ValidPassword_ShortNewPassword_ShouldThrowDomainException()
    {
        var user = User.Factory.Create("Luciano", "luciano", "123456");

        var ex = Assert.Throws<DomainException>(() =>
            user.ValidPassword("123"));
        Assert.Equal("A senha deve conter entre 6 e 100 caracteres.", ex.Message);
    }
}
