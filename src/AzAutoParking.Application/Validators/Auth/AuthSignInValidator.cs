using AzAutoParking.Application.Dto.Auth;
using FluentValidation;

namespace AzAutoParking.Application.Validators.Auth;

public class AuthSignInValidator : AbstractValidator<AuthSignInDto>
{
    public AuthSignInValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email completo é obrigatório")
            .EmailAddress().WithMessage("Email inválido");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Senha é obrigatório.")
            .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres.");
    }
}