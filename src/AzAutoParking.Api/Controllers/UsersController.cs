using System.Net;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzAutoParking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService service) : ControllerBase
    {
        private readonly IUserService _service = service;

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
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long id)
        {
            try
            {
                var response = await _service.GetByIdAsync(id);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserCreateDto userCreateDto)
        {
            try
            {
                var response = await _service.CreateAsync(userCreateDto);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UserUpdateDto userUpdateDto)
        {
            try
            {
                var response = await _service.UpdateAsync(userUpdateDto);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            try
            {
                var response = await _service.DeactiveAsync(id);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
    }
}
