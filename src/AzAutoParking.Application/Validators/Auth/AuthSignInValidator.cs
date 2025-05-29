using AzAutoParking.Application.Dto.Auth;
using AzAutoParking.Application.Response;
using FluentValidation;

namespace AzAutoParking.Application.Validators.Auth;

public class AuthSignInValidator : AbstractValidator<AuthSignInDto>
{
    public AuthSignInValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage($"{ErrorMessages.System.RequiredEmail.En} | {ErrorMessages.System.RequiredEmail.PtBr}")
            .EmailAddress()
            .WithMessage($"{ErrorMessages.System.InvalidEmail.En} | {ErrorMessages.System.InvalidEmail.PtBr}");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage($"{ErrorMessages.System.RequiredPassword.En} | {ErrorMessages.System.RequiredPassword.PtBr}");
    }
}