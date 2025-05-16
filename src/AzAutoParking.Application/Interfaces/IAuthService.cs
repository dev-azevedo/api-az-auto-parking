using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Response;

namespace AzAutoParking.Application.Interfaces;

public interface IAuthService
{
    Task<ResultResponse<UserGetDto>> SignIn(UserSignInDto userSignInDto);
    Task<ResultResponse<UserGetDto>> ConfirmAccountAsync(UserConfirmCodeDto userConfirmCodeDto);
    Task<ResultResponse<UserGetDto>> VerifyCode(UserConfirmCodeDto userConfirmCodeDto);
    Task<ResultResponse<bool>> ForgotPasswordAsync(string email);
    Task<ResultResponse<UserGetDto>> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto);
}