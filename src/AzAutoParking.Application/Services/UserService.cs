using System.Net;
using Mapster;
using AzAutoParking.Application.Dto;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Response;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Services;

public class UserService(IUserRepository repository, IJwtService jwtService, IEmailService emailService)
    : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly IEmailService _emailService = emailService;
    private readonly IPasswordHasherService _hasherService = new PasswordHasherService();

    public async Task<ResultResponse<PaginationDto<UserGetDto>>> GetAllAsync(int skip, int take)
    {
        var response = new ResultResponse<PaginationDto<UserGetDto>>();

        var (items, totalItems) = await _repository.GetAllAsync(skip, take);
        var userDto = items.Adapt<List<UserGetDto>>();
        
        var result = new PaginationDto<UserGetDto>
        {
            Items = userDto,
            TotalItems = totalItems,
        };

        return response.Success(HttpStatusCode.OK.GetHashCode(), result);
    }

    public async Task<ResultResponse<UserGetDto>> GetByIdAsync(long id)
    {
        var response = new ResultResponse<UserGetDto>();
        var userOnDb = await _repository.GetByIdAsync(id);

        if (userOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Auth.UserNotFound);
        
        var userDto = userOnDb.Adapt<UserGetDto>();
        
        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            userDto);
    }

    public async Task<ResultResponse<UserGetDto>> GetByEmailAsync(string email)
    {
        var response = new ResultResponse<UserGetDto>();
        var userOnDb = await _repository.GetByEmailAsync(email);

        if (userOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Auth.UserNotFound);
        
        var userDto = userOnDb.Adapt<UserGetDto>();
        return response.Success(HttpStatusCode.OK.GetHashCode(), userDto);
    }
    

    public async Task<ResultResponse<UserGetDto>> UpdateAsync(UserUpdateDto user)
    {
        var response = new ResultResponse<UserGetDto>();
        var userOnDb = await _repository.GetByIdAsync(user.Id);

        if (userOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Auth.UserNotFound);

        userOnDb.FullName = user.FullName;
        userOnDb.Email = user.Email;
        userOnDb.IsAdmin = user.IsAdmin ?? userOnDb.IsAdmin;
        userOnDb.IsModified();

        var userUpdate = await _repository.UpdateAsync(userOnDb);
        var userDto = userUpdate.Adapt<UserGetDto>();

        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            userDto);
    }

    
    public async Task<ResultResponse<bool>> DeactiveAsync(long id)
    {
        var response = new ResultResponse<bool>();
        var userOnDb = await _repository.GetByIdAsync(id);

        if (userOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Auth.UserNotFound);

        userOnDb.Deactivate();
        userOnDb.IsModified();
        await _repository.UpdateAsync(userOnDb);

        return response.Success(HttpStatusCode.NoContent.GetHashCode(), true);
    }

    public string GenerateRandomConfirmationCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] result = new char[8];

        for (int i = 0; i < 8; i++)
        {
            result[i] = chars[new Random().Next(chars.Length)];
        }
        
        return new string(result);
    }
}