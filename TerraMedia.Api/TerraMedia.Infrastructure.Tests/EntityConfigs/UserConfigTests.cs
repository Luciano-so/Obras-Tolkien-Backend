using Microsoft.EntityFrameworkCore;
using TerraMedia.Domain.Entities;
using TerraMedia.Infrastructure.Persistence.EntityConfigs;

namespace TerraMedia.Infrastructure.Tests.EntityConfigs;

public class UserConfigTests
{
    [Fact]
    public void Configure_ShouldSetupPropertiesAndRelationships()
    {
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        var config = new UserConfig();
        config.Configure(modelBuilder.Entity<User>());
        modelBuilder.Entity<BookComment>();

        var userEntityType = modelBuilder.Model.FindEntityType(typeof(User));
        var bookCommentEntityType = modelBuilder.Model.FindEntityType(typeof(BookComment));

        Assert.Equal("Users", userEntityType.GetTableName());

        var primaryKey = userEntityType.FindPrimaryKey();
        Assert.NotNull(primaryKey);
        Assert.Contains(userEntityType.FindProperty("Id"), primaryKey.Properties);

        var nameProperty = userEntityType.FindProperty("Name");
        Assert.False(nameProperty.IsNullable);
        Assert.Equal(250, nameProperty.GetMaxLength());

        var loginProperty = userEntityType.FindProperty("Login");
        Assert.False(loginProperty.IsNullable);
        Assert.Equal(50, loginProperty.GetMaxLength());

        var passwordProperty = userEntityType.FindProperty("Password");
        Assert.False(passwordProperty.IsNullable);
        Assert.Equal(100, passwordProperty.GetMaxLength());

        var createdAtProperty = userEntityType.FindProperty("CreatedAt");
        Assert.False(createdAtProperty.IsNullable);

        var activeProperty = userEntityType.FindProperty("Active");
        Assert.False(activeProperty.IsNullable);

        var foreignKeys = bookCommentEntityType.GetForeignKeys();
        var fkToUser = foreignKeys.FirstOrDefault(fk => fk.PrincipalEntityType.ClrType == typeof(User));

        Assert.NotNull(fkToUser);
        Assert.Equal(DeleteBehavior.Restrict, fkToUser.DeleteBehavior);
    }
}
