using System.Net;
using AutoMapper;
using AzAutoParking.Application.Dto;
using AzAutoParking.Application.Dto.Parking;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Response;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Services;

public class ParkingService(IParkingRepository repository, IMapper mapper) : IParkingService
{
    private readonly IParkingRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<ResultResponse<PaginationDto<ParkingGetDto>>> GetAllAsync(int skip, int take)
    {
        var response = new ResultResponse<PaginationDto<ParkingGetDto>>();

        var (items, totalItems) = await _repository.GetAllAsync(skip, take);
        var itemsResult = _mapper.Map<List<ParkingGetDto>>(items);
        var result = new PaginationDto<ParkingGetDto>
        {
            Items = itemsResult,
            TotalItems = totalItems,
        };

        return response.Success(HttpStatusCode.OK.GetHashCode(), result);
    }

    public async Task<ResultResponse<ParkingGetDto>> GetByIdAsync(long id)
    {
        var response = new ResultResponse<ParkingGetDto>();
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), "Parking not found");

        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            _mapper.Map<ParkingGetDto>(item));
    }

    public async Task<ResultResponse<ParkingGetDto>> GetByNumberSpaceAsync(int numberSpace)
    {
        var response = new ResultResponse<ParkingGetDto>();
        var item = await _repository.GetByParkingNumberAsync(numberSpace);

        if (item is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), "Parking not found");

        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            _mapper.Map<ParkingGetDto>(item));
    }

    public async Task<ResultResponse<ParkingGetDto>> CreateAsync(ParkingCreateDto parking)
    {
        var response = new ResultResponse<ParkingGetDto>();
        var parkingOnDb = await _repository.GetByParkingNumberAsync(parking.ParkingNumber);
        
        if(parkingOnDb is not null)
            return response.Fail(HttpStatusCode.Conflict.GetHashCode(), "Parking number already exists");

        var parkingModel = _mapper.Map<Parking>(parking);
        
        var parkingCreated = await _repository.CreateAsync(parkingModel);
        var parkingResponse = _mapper.Map<ParkingGetDto>(parkingCreated);

        return response.Success(
            HttpStatusCode.Created.GetHashCode(),
            parkingResponse);
    }

    public async Task<ResultResponse<ParkingGetDto>> UpdateAsync(ParkingUpdateDto parking)
    {
        var response = new ResultResponse<ParkingGetDto>();
        var parkingOnDb = await _repository.GetByIdAsync(parking.Id);
        if (parkingOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), "Parking not found");
        
        var parkingOnDbWithParkingNumber = await _repository.GetByParkingNumberAsync(parking.ParkingNumber);
        
        if(parkingOnDbWithParkingNumber is not null && parkingOnDb.Id != parkingOnDbWithParkingNumber.Id)
            return response.Fail(HttpStatusCode.Conflict.GetHashCode(), "Parking number already exists");
        
        parkingOnDb.ParkingNumber = parking.ParkingNumber;
        parkingOnDb.Available = parking.Available;
        parkingOnDb.Modified = DateTime.Now;

        var parkingUpdate = await _repository.UpdateAsync(parkingOnDb);
        var parkingResponse = _mapper.Map<ParkingGetDto>(parkingUpdate);

        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            parkingResponse);
    }

    public async Task<ResultResponse<bool>> DeactiveAsync(long id)
    {
        var response = new ResultResponse<bool>();
        var parkingOnDb = await _repository.GetByIdAsync(id);

        if (parkingOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), "Parking not found");

        parkingOnDb.IsActive = false;
        parkingOnDb.Modified = DateTime.Now;
        
        await _repository.UpdateAsync(parkingOnDb);

        return response.Success(HttpStatusCode.NoContent.GetHashCode(), true);
    }
}