using System.Net;
using AutoMapper;
using AzAutoParking.Application.Dto;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Response;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Services;

public class UserService(IUserRepository repository, IMapper mapper, IJwtService jwtService) : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IJwtService _jwtService = jwtService;
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
        {
            var message = "User not found";
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), message);
        }
        
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

    public async Task<ResultResponse<UserGetDto>> SignIn(UserSignInDto userSignInDto)
    {
        var response = new ResultResponse<UserGetDto>();
        var user = await _repository.GetByEmailAsync(userSignInDto.Email);
        if(user is null)
            return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), $"Email/Password invalid");
        
        var verifyPass = _hasherService.VerifyHashedPassword(userSignInDto.Password, user.Password);
        if(!verifyPass)
            return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), $"Email/Password invalid");
        
        // if(!user.ConfirmedAccount)
        //     return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), $"Verify email. Sent an email for confirmation of your account.");
        //     
        
        var token = _jwtService.GenerateJwtToken(user.Email, user.FullName, user.Id);
        
        var userDto = _mapper.Map<UserGetDto>(user);
        userDto.Token = token;
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), userDto);
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
        var userCreated = await _repository.CreateAsync(userModel);
        var userResponse = _mapper.Map<UserGetDto>(userCreated);
        
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
        await _repository.UpdateAsync(userOnDb);
        
        return response.Success(HttpStatusCode.NoContent.GetHashCode(), true);
    }
}