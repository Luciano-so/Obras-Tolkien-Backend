using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Infrastructure.Persistence.EntityConfigs;

public class BookCommentConfig : IEntityTypeConfiguration<BookComment>
{
    public void Configure(EntityTypeBuilder<BookComment> builder)
    {
        builder.ToTable("BookComments");

        builder.HasKey(bc => bc.Id);

        builder.Property(bc => bc.Id)
               .HasColumnName("Id")
               .IsRequired();

        builder.Property(bc => bc.UserId)
               .IsRequired();

        builder.Property(bc => bc.BookId)
               .IsRequired();

        builder.Property(bc => bc.Comment)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(bc => bc.CreatedAt)
               .IsRequired();

        builder.Property(bc => bc.UpdatedAt)
               .IsRequired();

        builder.HasOne(bc => bc.User)
               .WithMany()
               .HasForeignKey(bc => bc.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bc => bc.Book)
               .WithMany(b => b.Comments)
               .HasForeignKey(bc => bc.BookId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
