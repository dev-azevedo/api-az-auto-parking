using System.Net;
using AutoMapper;
using AzAutoParking.Application.Dto;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Response;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Services;

public class UserService(IUserRepository repository, IMapper mapper, IJwtService jwtService, IEmailService emailService)
    : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IEmailService _emailService = emailService;
    private readonly IPasswordHasherService _hasherService = new PasswordHasherService();

    public async Task<ResultResponse<PaginationDto<UserGetDto>>> GetAllAsync(int skip, int take)
    {
        var response = new ResultResponse<PaginationDto<UserGetDto>>();

        var (items, totalItems) = await _repository.GetAllAsync(skip, take);
        var itemsResult = _mapper.Map<List<UserGetDto>>(items);
        var result = new PaginationDto<UserGetDto>
        {
            Items = itemsResult,
            TotalItems = totalItems,
        };

        return response.Success(HttpStatusCode.OK.GetHashCode(), result);
    }

    public async Task<ResultResponse<UserGetDto>> GetByIdAsync(long id)
    {
        var response = new ResultResponse<UserGetDto>();
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), "User not found");

        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            _mapper.Map<UserGetDto>(item));
    }

    public async Task<ResultResponse<UserGetDto>> GetByEmailAsync(string email)
    {
        var response = new ResultResponse<UserGetDto>();
        var item = await _repository.GetByEmailAsync(email);

        if (item is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), $"User not found");

        return response.Success(HttpStatusCode.OK.GetHashCode(), _mapper.Map<UserGetDto>(item));
    }

    public async Task<ResultResponse<UserGetDto>> CreateAsync(UserCreateDto user)
    {
        var response = new ResultResponse<UserGetDto>();
        var userOnDb = await _repository.GetByEmailAsync(user.Email);

        if (userOnDb is not null)
            return response.Fail(HttpStatusCode.Conflict.GetHashCode(), $"Email already exists");

        var passwordHasher = _hasherService.HashPassword(user.Password);
        var userModel = _mapper.Map<User>(user);
        userModel.Password = passwordHasher;
        userModel.ConfirmationCode = GenerateRandomConfirmationCode();
        
        var userCreated = await _repository.CreateAsync(userModel);
        var userResponse = _mapper.Map<UserGetDto>(userCreated);

        var subject = "Bem-vindo ao AzAutoParking";
        var message = $@"
                        <p>Olá {userModel.FullName}, seja bem-vindo(a)</p>
                        <p>Confirme sua conta aqui: {userModel.ConfirmationCode}</p>
                        ";

        await _emailService.NotifyUserAsync(user.Email, subject, message: message);

        return response.Success(
            HttpStatusCode.Created.GetHashCode(),
            userResponse);
    }

    public async Task<ResultResponse<UserGetDto>> UpdateAsync(UserUpdateDto user)
    {
        var response = new ResultResponse<UserGetDto>();
        var userOnDb = await _repository.GetByIdAsync(user.Id);

        if (userOnDb is null)
        {
            var message = "User not found";
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), message);
        }

        userOnDb.FullName = user.FullName;
        userOnDb.Email = user.Email;
        userOnDb.IsAdmin = user.IsAdmin ?? userOnDb.IsAdmin;
        userOnDb.Modified = DateTime.Now;

        var userUpdate = await _repository.UpdateAsync(userOnDb);
        var userResponse = _mapper.Map<UserGetDto>(userUpdate);

        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            userResponse);
    }

    
    public async Task<ResultResponse<bool>> DeactiveAsync(long id)
    {
        var response = new ResultResponse<bool>();
        var userOnDb = await _repository.GetByIdAsync(id);

        if (userOnDb is null)
        {
            var message = "User not found";
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), message);
        }

        userOnDb.IsActive = false;
        userOnDb.Modified = DateTime.Now;
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