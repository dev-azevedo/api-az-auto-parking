using AzAutoParking.Infra.Data.Context;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace AzAutoParking.Api.Setup;

public static class DbSetup
{
    public static void AddDbSetup(this IServiceCollection services)
    {   
        var envDbName = Env.GetString("DB_NAME");
        var databasePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", envDbName);
        services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={databasePath}"));
    }
}