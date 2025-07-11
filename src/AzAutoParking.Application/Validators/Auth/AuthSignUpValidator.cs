using System.Text.RegularExpressions;
using AzAutoParking.Application.Dto.Auth;
using AzAutoParking.Application.Response;
using FluentValidation;

namespace AzAutoParking.Application.Validators.Auth;

public class AuthSignUpValidator : AbstractValidator<AuthSignUpDto>
{
    public AuthSignUpValidator()
    {
        RuleFor(u => u.FullName)
            .NotEmpty().WithMessage($"{ErrorMessages.Auth.RequiredFullName.En} | {ErrorMessages.Auth.RequiredFullName.PtBr}") //Full name is required
            .MinimumLength(5).WithMessage("Nome completo deve ter pelo menos 5 caracteres")
            .Must(fullname => !Regex.IsMatch(fullname, @"\d"))
            .WithMessage("Nome completo não pode conter números.");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage(
                $"{ErrorMessages.System.RequiredEmail.En} | {ErrorMessages.System.RequiredEmail.PtBr}") //Email is required
            .EmailAddress()
            .WithMessage($"{ErrorMessages.System.InvalidEmail.En} | {ErrorMessages.System.InvalidEmail.PtBr}");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage($"{ErrorMessages.System.RequiredPassword.En} | {ErrorMessages.System.RequiredPassword.PtBr}")
            .MinimumLength(8)
            .WithMessage($"{ErrorMessages.System.LongPassword.En} | {ErrorMessages.System.LongPassword.PtBr}")
            .Equal(c => c.ConfirmedPassword)
            .WithMessage($"{ErrorMessages.System.NotMatchConfirmPassword.En} | {ErrorMessages.System.NotMatchConfirmPassword.PtBr}");
    }
}
