using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Services;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using AzAutoParking.Infra.Data.Repository;
using AzAutoParking.Infra.ExternalServices;
using Mapster;
using MapsterMapper;


namespace AzAutoParking.Api.Setup;

public static class DependencyInjectionSetup
{
    public static void AddDiSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddServices();
        services.AddConfigureEnvs(configuration);
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IParkingRepository, ParkingRepository>();
        services.AddScoped<IAutomobileRepository, AutomobileRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IParkingService, ParkingService>();
        services.AddSingleton<IJwtService, JwtService>();
    }

    private static void AddConfigureEnvs(this IServiceCollection services, IConfiguration configuration)
    {
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