using System.Net;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzAutoParking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService service) : ControllerBase
    {
        private readonly IUserService _service = service;
        
        [Authorize]
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
        
        [Authorize]
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
        
        [Authorize]
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
        
        [Authorize(Policy = "IsAdmin")]
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
