using AzAutoParking.Application.Dto.Auth;
using FluentValidation;

namespace AzAutoParking.Application.Validators.Auth;

public class AuthConfirmCodeValidator : AbstractValidator<AuthConfirmCodeDto>
{
    public AuthConfirmCodeValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email completo é obrigatório")
            .EmailAddress().WithMessage("Email inválido");
        
        RuleFor(u => u.Code)
            .NotEmpty().WithMessage("O Código é obriagtório")
            .Length(8).WithMessage("O código dever ter pelo menos 8 caracteres.");
    }
}