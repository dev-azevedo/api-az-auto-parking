using AzAutoParking.Application.Dto;
using AzAutoParking.Application.Dto.Parking;
using AzAutoParking.Application.Response;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Interfaces;

public interface IParkingService
{
    Task<ResultResponse<PaginationDto<ParkingGetDto>>> GetAllAsync(int skip, int take);
    Task<ResultResponse<ParkingGetDto>> GetByIdAsync(long id);
    Task<ResultResponse<ParkingGetDto>> GetByNumberSpaceAsync(int numberSpace);
    Task<ResultResponse<ParkingGetDto>> CreateAsync(ParkingCreateDto parking);
    Task<ResultResponse<ParkingGetDto>> UpdateAsync(ParkingUpdateDto parking);
    Task<ResultResponse<bool>> DeactiveAsync(long id);
}