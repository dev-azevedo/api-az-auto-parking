using AzAutoParking.Application.Validators.Auth;
using AzAutoParking.Application.Validators.User;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace AzAutoParking.Api.Setup;

public static class FluentValidationSetup
{
    public static void AddFluentValidationSetup(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        
        // Auth
        services.AddValidatorsFromAssemblyContaining<AuthSignInValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthConfirmCodeValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthChangePasswordValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthResetPasswordValidator>();
        services.AddValidatorsFromAssemblyContaining<AuthForgotPasswordValidator>();
        
        // User
        services.AddValidatorsFromAssemblyContaining<UserCreateValidator>();
    }
}