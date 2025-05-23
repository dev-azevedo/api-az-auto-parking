using System.Text.RegularExpressions;
using AzAutoParking.Application.Dto.Auth;
using FluentValidation;

namespace AzAutoParking.Application.Validators.Auth;

public class AuthSignUpValidator : AbstractValidator<AuthSignUpDto>
{
    public AuthSignUpValidator()
    {
        RuleFor(u => u.FullName)
            .NotEmpty().WithMessage("Nome completo é obrigatório")
            .MinimumLength(5).WithMessage("Nome completo deve ter pelo menos 5 caracteres")
            .Must(fullname => !Regex.IsMatch(fullname, @"\d"))
            .WithMessage("Nome completo não pode conter números.");
        
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email é obrigatório.") //Email is required
            .EmailAddress().WithMessage("Email não é válido.");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Senha é obrigatório.")
            .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres.")
            .Equal(c => c.ConfirmedPassword).WithMessage("Senha não coincide com campos confirmar senha.");
    }
}