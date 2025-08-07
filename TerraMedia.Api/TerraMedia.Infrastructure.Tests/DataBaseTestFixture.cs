using Microsoft.EntityFrameworkCore;
using TerraMedia.Infrastructure.Persistence.Contexts;

namespace TerraMedia.Infrastructure.Tests;

public class DataBaseTestFixture : IDisposable
{
    public MainContext Context;

    public DataBaseTestFixture()
    {
        Context = CreateInMemoryContext();
    }

    private static MainContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<MainContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        return new MainContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
