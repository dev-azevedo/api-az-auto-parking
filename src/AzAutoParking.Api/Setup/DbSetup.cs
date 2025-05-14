using AzAutoParking.Infra.Data.Context;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace AzAutoParking.Api.Setup;

public static class DbSetup
{
    public static void AddDbSetup(this IServiceCollection services, IConfiguration configuration)
    {   
        var dbName = configuration["Database:Name"] ?? throw new ArgumentNullException("Database:Name");
        var databasePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", dbName);
        services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={databasePath}"));
    }
}