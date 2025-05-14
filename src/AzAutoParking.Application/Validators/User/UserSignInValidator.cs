using AzAutoParking.Application.Dto.User;
using FluentValidation;

namespace AzAutoParking.Application.Validators.User;

public class UserSignInValidator : AbstractValidator<UserSignInDto>
{
    public UserSignInValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email completo é obrigatório")
            .EmailAddress().WithMessage("Email inválido");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Senha é obrigatório.")
            .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres.");
    }
}