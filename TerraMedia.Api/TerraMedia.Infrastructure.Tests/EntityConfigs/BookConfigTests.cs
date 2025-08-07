using Microsoft.EntityFrameworkCore;
using TerraMedia.Domain.Entities;
using TerraMedia.Infrastructure.Persistence.EntityConfigs;

namespace TerraMedia.Infrastructure.Tests.EntityConfigs;

public class BookConfigTests
{
    [Fact]
    public void Configure_ShouldSetupPropertiesAndRelationships()
    {
        var modelBuilder = new ModelBuilder(new Microsoft.EntityFrameworkCore.Metadata.Conventions.ConventionSet());

        var config = new BookConfig();
        config.Configure(modelBuilder.Entity<Book>());

        var entityType = modelBuilder.Model.FindEntityType(typeof(Book));

        Assert.Equal("Books", entityType.GetTableName());

        var primaryKey = entityType.FindPrimaryKey();
        Assert.NotNull(primaryKey);
        Assert.Contains(entityType.FindProperty("Id"), primaryKey.Properties);

        Assert.False(entityType.FindProperty("Id").IsNullable);
        Assert.False(entityType.FindProperty("CoverId").IsNullable);
        Assert.False(entityType.FindProperty("CreatedAt").IsNullable);


        var navigation = entityType.FindNavigation("Comments");
        Assert.NotNull(navigation);
        Assert.True(navigation.IsCollection);

        var foreignKey = navigation.ForeignKey;
        Assert.Equal("BookId", foreignKey.Properties[0].Name);
        Assert.Equal(DeleteBehavior.Cascade, foreignKey.DeleteBehavior);
    }
}
