using System.Net;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzAutoParking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _service = authService;
        
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
        
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmAccountAsync([FromBody] UserConfirmCodeDto userConfirmCodeDto)
        {
            try
            {
                var response = await _service.ConfirmAccountAsync(userConfirmCodeDto);
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
    }
}
