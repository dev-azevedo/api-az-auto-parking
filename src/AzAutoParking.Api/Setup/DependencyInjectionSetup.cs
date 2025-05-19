using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Mapper;
using AzAutoParking.Application.Services;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Infra.Data.Repository;
using AzAutoParking.Infra.ExternalServices;

namespace AzAutoParking.Api.Setup;

public static class DependencyInjectionSetup
{
    public static void AddDiSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddServices(configuration);
        services.AddAutoMapper(typeof(AutoMapperConfig));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IParkingRepository, ParkingRepository>();
    }

    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IParkingService, ParkingService>();
        
        var emailSmtpHost = configuration["Email:SmtpHost"] ?? throw new ArgumentNullException("Email:SmtpHost");
        services.AddSingleton<ISmtpEmailService>(provider =>
            new SmtpEmailService(
                smtpHost: emailSmtpHost,
                smtpPort: int.Parse(configuration["Email:SmtpPort"]), 
                from: configuration["Email:From"],
                nameFrom: configuration["Email:NameFrom"],
                password: configuration["Email:Password"]
            )
        );
    }
}