using AzAutoParking.Application.Dto.Auth;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Response;

namespace AzAutoParking.Application.Interfaces;

public interface IAuthService
{
    Task<ResultResponse<UserGetDto>> SignInAsync(AuthSignInDto authSignInDto);
    Task<ResultResponse<UserGetDto>> SignUpAsync(AuthSignUpDto user);
    Task<ResultResponse<UserGetDto>> ConfirmAccountAsync(AuthConfirmCodeDto authConfirmCodeDto);
    Task<ResultResponse<UserGetDto>> VerifyCode(AuthConfirmCodeDto authConfirmCodeDto);
    Task<ResultResponse<bool>> ForgotPasswordAsync(string email);
    Task<ResultResponse<bool>> ResetPasswordAsync(AuthResetPasswordDto authResetPasswordDto);
    Task<ResultResponse<UserGetDto>> ChangePasswordAsync(AuthChangePasswordDto authChangePasswordDto);
}