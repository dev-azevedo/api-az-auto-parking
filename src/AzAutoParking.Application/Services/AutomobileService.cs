using System.Net;
using AzAutoParking.Application.Dto;
using AzAutoParking.Application.Dto.Automobile;
using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Response;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using Mapster;

namespace AzAutoParking.Application.Services;

public class AutomobileService(IAutomobileRepository repository) : IAutomobileService
{
    private readonly IAutomobileRepository _repository = repository;
    
    public async Task<ResultResponse<PaginationDto<AutomobileGetDto>>> GetAllAsync(int skip, int take)
    {
        var response = new ResultResponse<PaginationDto<AutomobileGetDto>>();

        var (items, totalItems) = await _repository.GetAllAsync(skip, take);
        var AutomobileDto = items.Adapt<List<AutomobileGetDto>>();
        
        var result = new PaginationDto<AutomobileGetDto>
        {
            Items = AutomobileDto,
            TotalItems = totalItems,
        };

        return response.Success(HttpStatusCode.OK.GetHashCode(), result);
    }

    public async Task<ResultResponse<AutomobileGetDto>> GetByIdAsync(long id)
    {
        var response = new ResultResponse<AutomobileGetDto>();
        var automobileOnDb = await _repository.GetByIdAsync(id);

        if (automobileOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), [ErrorMessages.Parking.NotFound]);
        
        var automobileDto = automobileOnDb.Adapt<AutomobileGetDto>();
        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            automobileDto);
    }


    public async Task<ResultResponse<AutomobileGetDto>> CreateAsync(AutomobileCreateDto automobile)
    {
        var response = new ResultResponse<AutomobileGetDto>();
        
        // Get for plate car
        // var parkingOnDb = await _repository.GetByParkingNumberAsync(parking.ParkingNumber);
        //
        // if(parkingOnDb is not null)
        //     return response.Fail(HttpStatusCode.Conflict.GetHashCode(), [ErrorMessages.Parking.ParkingNumberExists]);

        var automobileModel = automobile.Adapt<Automobile>();
        
        var automobileCreated = await _repository.CreateAsync(automobileModel);
        var automobileDto = automobileCreated.Adapt<AutomobileGetDto>();

        return response.Success(
            HttpStatusCode.Created.GetHashCode(),
            automobileDto);
    }

    public async Task<ResultResponse<AutomobileGetDto>> UpdateAsync(AutomobileUpdateDto automobile)
    {
        var response = new ResultResponse<AutomobileGetDto>();
        var automobileOnDb = await _repository.GetByIdAsync(automobile.Id);
        if (automobileOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), [ErrorMessages.Parking.NotFound]);
        
        automobileOnDb.Brand = automobileOnDb.Brand;
        automobileOnDb.Model = automobileOnDb.Model;
        automobileOnDb.Color = automobileOnDb.Color;
        automobileOnDb.Contact = automobileOnDb.Contact;
        automobileOnDb.Client = automobileOnDb.Client;
        automobileOnDb.Plate = automobileOnDb.Plate;
        automobileOnDb.TypeAutomobile = automobileOnDb.TypeAutomobile;

        var automobileUpdate = await _repository.UpdateAsync(automobileOnDb);
        var automobileDto = automobileUpdate.Adapt<AutomobileGetDto>();

        return response.Success(
            HttpStatusCode.OK.GetHashCode(),
            automobileDto);
    }

    public async Task<ResultResponse<bool>> DeactiveAsync(long id)
    {
        var response = new ResultResponse<bool>();
        var automobileOnDb = await _repository.GetByIdAsync(id);

        if (automobileOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), [ErrorMessages.Parking.NotFound]);

        automobileOnDb.Deactivate();
        automobileOnDb.IsModified();
        
        await _repository.UpdateAsync(automobileOnDb);

        return response.Success(HttpStatusCode.NoContent.GetHashCode(), true);
    }
}