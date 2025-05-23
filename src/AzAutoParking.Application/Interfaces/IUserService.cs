﻿using AzAutoParking.Application.Dto;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Response;

namespace AzAutoParking.Application.Interfaces;

public interface IUserService
{
    Task<ResultResponse<PaginationDto<UserGetDto>>> GetAllAsync(int skip, int take);
    Task<ResultResponse<UserGetDto>> GetByIdAsync(long id);
    Task<ResultResponse<UserGetDto>> GetByEmailAsync(string email);
    Task<ResultResponse<UserGetDto>> UpdateAsync(UserUpdateDto user);
    Task<ResultResponse<bool>> DeactiveAsync(long id);
    string GenerateRandomConfirmationCode();

}