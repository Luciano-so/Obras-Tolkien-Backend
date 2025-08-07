using Microsoft.EntityFrameworkCore;
using TerraMedia.Domain.Entities;
using TerraMedia.Infrastructure.Persistence.EntityConfigs;

namespace TerraMedia.Infrastructure.Tests.EntityConfigs;

public class BookCommentConfigTests
{
    [Fact]
    public void Configure_ShouldSetupPropertiesAndRelationships()
    {
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        var config = new BookCommentConfig();
        config.Configure(modelBuilder.Entity<BookComment>());

        var entityType = modelBuilder.Model.FindEntityType(typeof(BookComment));

        var primaryKey = entityType.FindPrimaryKey();
        Assert.NotNull(primaryKey);
        Assert.Contains(entityType.FindProperty("Id"), primaryKey.Properties);

        Assert.True(entityType.FindProperty("UserId").IsNullable == false);
        Assert.True(entityType.FindProperty("BookId").IsNullable == false);
        Assert.True(entityType.FindProperty("Comment").IsNullable == false);
        Assert.Equal(1000, entityType.FindProperty("Comment").GetMaxLength());

        var userFk = entityType.FindForeignKeys(entityType.FindProperty("UserId"));
        Assert.NotEmpty(userFk);

        var bookFk = entityType.FindForeignKeys(entityType.FindProperty("BookId"));
        Assert.NotEmpty(bookFk);

        var userDeleteBehavior = userFk.First().DeleteBehavior;
        var bookDeleteBehavior = bookFk.First().DeleteBehavior;

        Assert.Equal(DeleteBehavior.Restrict, userDeleteBehavior);
        Assert.Equal(DeleteBehavior.Cascade, bookDeleteBehavior);
    }
}
