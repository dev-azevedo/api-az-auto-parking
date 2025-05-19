using AzAutoParking.Application.Dto.Parking;
using AzAutoParking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzAutoParking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParkingsController(IParkingService service) : ControllerBase
    {
        private readonly IParkingService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int skip = 1, int take = 10)
        {
            try
            {
                var response = await _service.GetAllAsync(skip, take);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var response = await _service.GetByIdAsync(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("number/{parkingNumber}")]
        public async Task<IActionResult> GetByParkingNumberAsync([FromRoute] int parkingNumber)
        {
            try
            {
                var response = await _service.GetByParkingNumberAsync(parkingNumber);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ParkingCreateDto parking)
        {
            try
            {
                var response = await _service.CreateAsync(parking);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] ParkingUpdateDto parking)
        {
            try
            {
                var response = await _service.UpdateAsync(parking);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [Authorize (Policy = "IsAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactiveAsync([FromRoute] long id)
        {
            try
            {
                var response = await _service.DeactiveAsync(id);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
