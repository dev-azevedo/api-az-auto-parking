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

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync(UserSignInDto userSignInDto)
        {
            try
            {
                var response = await _service.SignIn(userSignInDto);
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

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmAccountAsync([FromBody] UserConfirmAccountDto userConfirmAccountDto)
        {
            try
            {
                var response = await _service.ConfirmAccountAsync(userConfirmAccountDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
        
        [Authorize]
        [HttpPost("change/password")]
        public async Task<IActionResult> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto)
        {
            try
            {
                var response = await _service.ChangePasswordAsync(userChangePasswordDto);
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
