using System.Text.RegularExpressions;
using AzAutoParking.Application.Dto.User;
using FluentValidation;

namespace AzAutoParking.Application.Validator.User;

public class UserCreateValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateValidator()
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
            .Length(8).WithMessage("Senha deve ter pelo menos 8 caracteres.")
            .Equal(c => c.ConfirmedPassword).WithMessage("Senha não coincide com campos confirmar senha.");
    }
}