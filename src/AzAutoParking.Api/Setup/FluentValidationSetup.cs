using AzAutoParking.Application.Validators.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace AzAutoParking.Api.Setup;

public static class FluentValidationSetup
{
    public static void AddFluentValidationSetup(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        // services.AddFluentValidationClientsideAdapters();
 
        // Auth
        services.AddValidatorsFromAssemblyContaining<AuthSignInValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthSignUpValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthConfirmCodeValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthChangePasswordValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthResetPasswordValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthForgotPasswordValidator>();
        
    }
}