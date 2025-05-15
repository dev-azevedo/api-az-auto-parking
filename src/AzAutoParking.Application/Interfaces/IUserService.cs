using AzAutoParking.Application.Dto;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Response;

namespace AzAutoParking.Application.Interfaces;

public interface IUserService
{
    Task<ResultResponse<PaginationDto<UserGetDto>>> GetAllAsync(int skip, int take);
    Task<ResultResponse<UserGetDto>> GetByIdAsync(long id);
    Task<ResultResponse<UserGetDto>> GetByEmailAsync(string email);
    Task<ResultResponse<UserGetDto>> SignIn(UserSignInDto userSignInDto);
    Task<ResultResponse<UserGetDto>> CreateAsync(UserCreateDto user);
    Task<ResultResponse<UserGetDto>> UpdateAsync(UserUpdateDto user);
    Task<ResultResponse<UserGetDto>> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto);
    Task<ResultResponse<UserGetDto>> ConfirmAccountAsync(UserConfirmAccountDto userConfirmAccountDto);
    Task<ResultResponse<bool>> DeactiveAsync(long id);
    
}