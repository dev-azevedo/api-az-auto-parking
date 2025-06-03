using AzAutoParking.Application.Dto;
using AzAutoParking.Application.Dto.Automobile;
using AzAutoParking.Application.Response;

namespace AzAutoParking.Application.Interfaces;

public interface IAutomobileService
{
    Task<ResultResponse<PaginationDto<AutomobileGetDto>>> GetAllAsync(int skip, int take);
    Task<ResultResponse<AutomobileGetDto>> GetByIdAsync(long id);
    Task<ResultResponse<AutomobileGetDto>> CreateAsync(AutomobileCreateDto automobile);
    Task<ResultResponse<AutomobileGetDto>> UpdateAsync(AutomobileUpdateDto automobile);
    Task<ResultResponse<bool>> DeactiveAsync(long id);
}