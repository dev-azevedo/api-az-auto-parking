using AzAutoParking.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AzAutoParking.Tests.Infra.Data.Context;

public class AppDbContextInMemory
{
    public AppDbContext CreateDbContextInMemory()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}