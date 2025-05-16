using AzAutoParking.Application.Dto.User;
using FluentValidation;

namespace AzAutoParking.Application.Validators.User;

public class UserConfirmAccountValidator : AbstractValidator<UserConfirmCodeDto>
{
    public UserConfirmAccountValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email é obrigatório.") //Email is required
            .EmailAddress().WithMessage("Email não é válido.");
        RuleFor(u => u.Code)
            .NotEmpty().WithMessage("O Código é obriagtório")
            .Length(8).WithMessage("O código dever ter pelo menos 8 caracteres.");
    }
}