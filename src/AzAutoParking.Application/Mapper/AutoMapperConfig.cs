using AutoMapper;
using AzAutoParking.Application.Dto.Parking;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Mapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        // User
        CreateMap<User, UserGetDto>().ReverseMap();
        CreateMap<User, UserCreateDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
        
        //Parking
        CreateMap<Parking, ParkingGetDto>().ReverseMap();
        CreateMap<Parking, ParkingCreateDto>().ReverseMap();
    }
}