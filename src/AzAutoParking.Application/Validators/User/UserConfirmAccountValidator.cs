using AzAutoParking.Application.Dto.User;
using FluentValidation;

namespace AzAutoParking.Application.Validators.User;

public class UserConfirmAccountValidator : AbstractValidator<UserConfirmAccountDto>
{
    public UserConfirmAccountValidator()
    {
        RuleFor(c => c.Id).GreaterThan(0).WithMessage("O Id deve ser maior que zero.");
        RuleFor(c => c.Code)
            .NotEmpty().WithMessage("O Código é obriagtório")
            .Length(8).WithMessage("O código dever ter pelo menos 8 caracteres.");
    }
}