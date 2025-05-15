using AzAutoParking.Application.Validators.User;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace AzAutoParking.Api.Setup;

public static class FluentValidationSetup
{
    public static void AddFluentValidationSetup(this IServiceCollection services)
    {
        // User
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<UserCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<UserSignInValidator>();
        services.AddValidatorsFromAssemblyContaining<UserConfirmAccountValidator>();
        services.AddValidatorsFromAssemblyContaining<UserChangePasswordValidator>();
    }
}