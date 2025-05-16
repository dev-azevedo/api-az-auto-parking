using System.Net;
using AutoMapper;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Response;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Services;

public class AuthService(IMapper mapper, IJwtService jwtService, IUserRepository userRepository, IUserService userService, IEmailService emailService) : IAuthService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IUserService _userService = userService;
    private readonly IEmailService _emailService = emailService;
    private readonly IPasswordHasherService _hasherService = new PasswordHasherService();
    
    
    public async Task<ResultResponse<UserGetDto>> SignIn(UserSignInDto userSignInDto)
    {
        var response = new ResultResponse<UserGetDto>();
        var userOnDb = await _userRepository.GetByEmailAsync(userSignInDto.Email);
        if (userOnDb is null)
            return response.Fail(HttpStatusCode.BadRequest.GetHashCode(), "Email/Password invalid");
        
        var verifyPass = _hasherService.VerifyHashedPassword(userSignInDto.Password, userOnDb.Password);
        if (!verifyPass)
            return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), $"Email/Password invalid");

        if(!userOnDb.ConfirmedAccount)
            return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), $"Verify email. Sent an email for confirmation of your account.");


        var token = _jwtService.GenerateJwtToken(userOnDb.Id, userOnDb.Email, userOnDb.FullName, userOnDb.IsAdmin);

        var userDto = _mapper.Map<UserGetDto>(userOnDb);
        userDto.Token = token;

        return response.Success(HttpStatusCode.OK.GetHashCode(), userDto);
    }
    
    public async Task<ResultResponse<UserGetDto>> ConfirmAccountAsync(UserConfirmCodeDto userConfirmCodeDto)
    {
        var response = new ResultResponse<UserGetDto>();
        var userResponse = await _validateCode(userConfirmCodeDto);
        if (!userResponse.IsSuccess)
        {
            var message = userResponse.Message ?? "Invalid code";
            return response.Fail(userResponse.StatusCode, message);
        }
        
        var userOnDb = userResponse.Data;
        userOnDb.ConfirmedAccount = true;
        userOnDb.Modified = DateTime.Now;
        
        var userUpdate = await _userRepository.UpdateAsync(userOnDb);
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), _mapper.Map<UserGetDto>(userUpdate));
    }
    
    public async Task<ResultResponse<UserGetDto>> VerifyCode(UserConfirmCodeDto userConfirmCodeDto)
    {
        var response = new ResultResponse<UserGetDto>();
        var userResponse = await _validateCode(userConfirmCodeDto);
       
        if (!userResponse.IsSuccess)
        {
            var message = userResponse.Message ?? "Invalid code";
            return response.Fail(userResponse.StatusCode, message);
        }

        var userOnDb = userResponse.Data;
        
        var token = _jwtService.GenerateJwtToken(userOnDb.Id, userOnDb.Email, userOnDb.FullName, userOnDb.IsAdmin, 15);

        var userDto = _mapper.Map<UserGetDto>(userOnDb);
        userDto.Token = token;
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), _mapper.Map<UserGetDto>(userDto));
    }
    
    public async Task<ResultResponse<bool>> ForgotPasswordAsync(string email)
    {
        var response = new ResultResponse<bool>();
        var userOnDb = await _userRepository.GetByEmailAsync(email);
        if (userOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), "User not found");

        if (userOnDb.ConfirmationCode is not null)
            return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), $"Verify email. Sent an email for confirmation of code.");
        
        
        userOnDb.ConfirmationCode = _userService.GenerateRandomConfirmationCode();
        
        var subject = "Bem-vindo ao AzAutoParking";
        var messageMail = $@"
                        <p>Olá {userOnDb.FullName}, seja bem-vindo(a)</p>
                        <p>Confirme sua conta aqui: {userOnDb.ConfirmationCode}</p>
                        ";

        await _emailService.NotifyUserAsync(userOnDb.Email, subject, message: messageMail);
        return response.Success(HttpStatusCode.NoContent.GetHashCode(), true);
    }
    
    public async Task<ResultResponse<UserGetDto>> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto)
    {
        var response = new ResultResponse<UserGetDto>();
        var userOnDb = await _userRepository.GetByIdAsync(userChangePasswordDto.Id);
        if (userOnDb is null)
        {
            var message = "User not found";
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), message);
        }
        
        
        var verifyPass = _hasherService.VerifyHashedPassword(userChangePasswordDto.OldPassword, userOnDb.Password);
        if (!verifyPass)
            return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), $"Old Password invalid");
        
        var passwordHasher = _hasherService.HashPassword(userChangePasswordDto.NewPassword);
        userOnDb.Password = passwordHasher;
        userOnDb.Modified = DateTime.Now;
        
        var userUpdated = await _userRepository.UpdateAsync(userOnDb);
        var userResponse = _mapper.Map<UserGetDto>(userUpdated);
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), userResponse);
    }

    
    private async Task<ResultResponse<User>> _validateCode(UserConfirmCodeDto userConfirmCode)
    {
        var response = new ResultResponse<User>();
        var userOnDb = await _userRepository.GetByEmailAsync(userConfirmCode.Email);
        if (userOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), "User not found");
        

        if(userOnDb.ConfirmationCode != userConfirmCode.Code)
            return response.Fail(HttpStatusCode.BadRequest.GetHashCode(), "Invalid code");
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), userOnDb);
    }
}