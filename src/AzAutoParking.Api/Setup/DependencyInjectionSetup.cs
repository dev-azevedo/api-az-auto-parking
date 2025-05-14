using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Mapper;
using AzAutoParking.Application.Services;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Infra.Data.Repository;

namespace AzAutoParking.Api.Setup;

public static class DependencyInjectionSetup
{
    public static void AddDiSetup(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddServices();
        services.AddAutoMapper(typeof(AutoMapperConfig));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IJwtService, JwtService>();
    }
}