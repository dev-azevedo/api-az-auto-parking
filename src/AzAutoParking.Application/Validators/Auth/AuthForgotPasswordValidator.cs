using AzAutoParking.Application.Dto.Auth;
using FluentValidation;

namespace AzAutoParking.Application.Validators.Auth;

public class AuthForgotPasswordValidator : AbstractValidator<AuthForgotPasswordDto>
{
    public AuthForgotPasswordValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email completo é obrigatório")
            .EmailAddress().WithMessage("Email inválido");
    }
}