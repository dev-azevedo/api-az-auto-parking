using System.Net;
using AutoMapper;
using AzAutoParking.Application.Dto;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Response;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Services;

public class UserService(IUserRepository repository, IMapper mapper) : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

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

    public Task<ResultResponse<UserGetDto>> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultResponse<UserGetDto>> CreateAsync(UserCreateDto user)
    {
        var response = new ResultResponse<UserGetDto>();
        var userModel = _mapper.Map<User>(user);
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