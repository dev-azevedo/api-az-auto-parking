using AutoMapper;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Mapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<User, UserGetDto>().ReverseMap();
        CreateMap<User, UserCreateDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
    }
}