using System.Net;
using Mapster;
using AzAutoParking.Application.Dto;
using AzAutoParking.Application.Dto.Parking;
using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Response;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Services;

public class ParkingService(IParkingRepository repository) : IParkingService
{
    private readonly IParkingRepository _repository = repository;
    
    public async Task<ResultResponse<PaginationDto<ParkingGetDto>>> GetAllAsync(int skip, int take)
    {
        var response = new ResultResponse<PaginationDto<ParkingGetDto>>();

        var (items, totalItems) = await _repository.GetAllAsync(skip, take);
        var parkingDto = items.Adapt<List<ParkingGetDto>>();
        
        var result = new PaginationDto<ParkingGetDto>
        {
            Items = parkingDto,
            TotalItems = totalItems,
        };

        return response.Success(HttpStatusCode.OK.GetHashCode(), result);
    }

    public async Task<ResultResponse<ParkingGetDto>> GetByIdAsync(long id)
    {
        var response = new ResultResponse<ParkingGetDto>();
        var parkingOnDb = await _repository.GetByIdAsync(id);

        if (parkingOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Parking.NotFound);
        
        var parkingDto = parkingOnDb.Adapt<ParkingGetDto>();
        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            parkingDto);
    }

    public async Task<ResultResponse<ParkingGetDto>> GetByParkingNumberAsync(int numberSpace)
    {
        var response = new ResultResponse<ParkingGetDto>();
        var parkingOnDb = await _repository.GetByParkingNumberAsync(numberSpace);

        if (parkingOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Parking.NotFound);
        
        var parkingDto = parkingOnDb.Adapt<ParkingGetDto>();
        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            parkingDto);
    }

    public async Task<ResultResponse<ParkingGetDto>> CreateAsync(ParkingCreateDto parking)
    {
        var response = new ResultResponse<ParkingGetDto>();
        var parkingOnDb = await _repository.GetByParkingNumberAsync(parking.ParkingNumber);
        
        if(parkingOnDb is not null)
            return response.Fail(HttpStatusCode.Conflict.GetHashCode(), ErrorMessages.Parking.ParkingNumberExists);

        var parkingModel = parking.Adapt<Parking>();
        
        var parkingCreated = await _repository.CreateAsync(parkingModel);
        var parkingDto = parkingCreated.Adapt<ParkingGetDto>();

        return response.Success(
            HttpStatusCode.Created.GetHashCode(),
            parkingDto);
    }

    public async Task<ResultResponse<ParkingGetDto>> UpdateAsync(ParkingUpdateDto parking)
    {
        var response = new ResultResponse<ParkingGetDto>();
        var parkingOnDb = await _repository.GetByIdAsync(parking.Id);
        if (parkingOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Parking.NotFound);
        
        var parkingOnDbWithParkingNumber = await _repository.GetByParkingNumberAsync(parking.ParkingNumber);
        
        if(parkingOnDbWithParkingNumber is not null && parkingOnDb.Id != parkingOnDbWithParkingNumber.Id)
            return response.Fail(HttpStatusCode.Conflict.GetHashCode(), ErrorMessages.Parking.ParkingNumberExists);
        
        parkingOnDb.ParkingNumber = parking.ParkingNumber;
        parkingOnDb.Available = parking.Available;
        parkingOnDb.IsModified();

        var parkingUpdate = await _repository.UpdateAsync(parkingOnDb);
        var parkingDto = parkingUpdate.Adapt<ParkingGetDto>();

        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            parkingDto);
    }

    public async Task<ResultResponse<bool>> DeactiveAsync(long id)
    {
        var response = new ResultResponse<bool>();
        var parkingOnDb = await _repository.GetByIdAsync(id);

        if (parkingOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Parking.NotFound);

        parkingOnDb.Deactivate();
        parkingOnDb.IsModified();
        
        await _repository.UpdateAsync(parkingOnDb);

        return response.Success(HttpStatusCode.NoContent.GetHashCode(), true);
    }
}