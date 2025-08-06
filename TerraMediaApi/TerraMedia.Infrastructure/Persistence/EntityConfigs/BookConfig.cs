using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Infrastructure.Persistence.EntityConfigs;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
               .IsRequired();

        builder.Property(b => b.CoverId)
               .IsRequired();

        builder.Property(b => b.CreatedAt)
               .IsRequired();

        builder.HasMany(b => b.Comments)
               .WithOne(c => c.Book)
               .HasForeignKey(c => c.BookId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
