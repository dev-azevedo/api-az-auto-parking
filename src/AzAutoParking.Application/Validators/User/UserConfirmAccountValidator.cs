using AzAutoParking.Application.Dto.User;
using FluentValidation;

namespace AzAutoParking.Application.Validators.User;

public class UserConfirmAccountValidator : AbstractValidator<UserConfirmAccountDto>
{
    public UserConfirmAccountValidator()
    {
        RuleFor(u => u.Id).GreaterThan(0).WithMessage("O Id deve ser maior que zero.");
        RuleFor(u => u.Code)
            .NotEmpty().WithMessage("O Código é obriagtório")
            .Length(8).WithMessage("O código dever ter pelo menos 8 caracteres.");
    }
}