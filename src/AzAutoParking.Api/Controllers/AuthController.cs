using System.Net;
using AzAutoParking.Application.Dto.Auth;
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
        public async Task<IActionResult> SignInAsync(AuthSignInDto authSignInDto)
        {
            try
            {
                var response = await _service.SignIn(authSignInDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
        
        [HttpPost("verify/account")]
        public async Task<IActionResult> ConfirmAccountAsync([FromBody] AuthConfirmCodeDto authConfirmCodeDto)
        {
            try
            {
                var response = await _service.ConfirmAccountAsync(authConfirmCodeDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [HttpPost("verify/code")]
        public async Task<IActionResult> ConfirmCodeAsync(AuthConfirmCodeDto authConfirmCodeDto)
        {
            try
            {
                var response = await _service.VerifyCode(authConfirmCodeDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
        
        [HttpPost("password/forgot")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] AuthForgotPasswordDto authForgotPasswordDto)
        {
            try
            {
                
                var response = await _service.ForgotPasswordAsync(authForgotPasswordDto.Email);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Authorize]
        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] AuthResetPasswordDto authResetPasswordDto)
        {
            try
            {
                var response = await _service.ResetPasswordAsync(authResetPasswordDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
            
        }
        
        [Authorize]
        [HttpPost("password/change")]
        public async Task<IActionResult> ChangePasswordAsync(AuthChangePasswordDto authChangePasswordDto)
        {
            try
            {
                var response = await _service.ChangePasswordAsync(authChangePasswordDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
        
    }
}
