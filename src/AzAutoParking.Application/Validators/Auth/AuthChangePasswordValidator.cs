﻿using AzAutoParking.Application.Dto.Auth;
using FluentValidation;

namespace AzAutoParking.Application.Validators.Auth;

public class AuthChangePasswordValidator : AbstractValidator<AuthChangePasswordDto>
{
    public AuthChangePasswordValidator()
    {
        RuleFor(u => u.Id).GreaterThan(0).WithMessage("O Id deve ser maior que zero.");
        RuleFor(u => u.OldPassword)
            .NotEmpty().WithMessage("Senha antiga deve ser informado.");
        
        RuleFor(u => u.NewPassword)
            .NotEmpty().WithMessage("Senha nova deve ser informado.")
            .MinimumLength(8).WithMessage("Senha nova deve ter pelo menos 8 caracteres.")
            .Equal(u => u.ConfirmedNewPassword).WithMessage("Senha nova não coincide com campos confirmar senha.");
    }
    
}