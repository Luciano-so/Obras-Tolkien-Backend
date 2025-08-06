using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Infrastructure.Persistence.EntityConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .IsRequired();

        builder.Property(u => u.Name)
               .HasMaxLength(250)
               .IsRequired();

        builder.Property(u => u.Login)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(u => u.Password)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.CreatedAt)
               .IsRequired();

        builder.Property(u => u.Active)
               .IsRequired();

        builder.HasMany<BookComment>()
               .WithOne(c => c.User!)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
