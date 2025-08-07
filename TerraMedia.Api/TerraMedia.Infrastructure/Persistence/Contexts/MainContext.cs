
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TerraMedia.Domain.Contracts.Data;
using TerraMedia.Domain.Entities;

namespace TerraMedia.Infrastructure.Persistence.Contexts;

public class MainContext : DbContext, IUnitOfWork
{
    public DbSet<Book> Books { get; set; }
    public DbSet<BookComment> BookComments { get; set; }
    public DbSet<User> Users { get; set; }
    public MainContext(DbContextOptions<MainContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        var entitiesAdded = new List<EntityEntry>();
        var entitiesModified = new List<EntityEntry>();
        var entitiesDeleted = new List<EntityEntry>();

        foreach (var entry in this.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added: entitiesAdded.Add(entry); break;
                case EntityState.Modified: entitiesModified.Add(entry); break;
                case EntityState.Deleted: entitiesDeleted.Add(entry); break;
            }
        }
        return await base.SaveChangesAsync() > 0;
    }
}
