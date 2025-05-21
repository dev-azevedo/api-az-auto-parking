using System.Net;
using AutoMapper;
using AzAutoParking.Application.Dto.Auth;
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
    
    
    public async Task<ResultResponse<UserGetDto>> SignIn(AuthSignInDto authSignInDto)
    {
        var response = new ResultResponse<UserGetDto>();
        var userOnDb = await _userRepository.GetByEmailAsync(authSignInDto.Email);
        if (userOnDb is null)
            return response.Fail(HttpStatusCode.BadRequest.GetHashCode(), ErrorMessages.Auth.InvalidEmailOrPassword);
        
        var verifyPass = _hasherService.VerifyHashedPassword(authSignInDto.Password, userOnDb.Password);
        if (!verifyPass)
            return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), ErrorMessages.Auth.InvalidEmailOrPassword);

        if(!userOnDb.ConfirmedAccount)
            return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), ErrorMessages.Auth.UnconfirmedAccount);


        var token = _jwtService.GenerateJwtToken(userOnDb.Id, userOnDb.Email, userOnDb.FullName, userOnDb.IsAdmin);

        var userDto = _mapper.Map<UserGetDto>(userOnDb);
        userDto.Token = token;

        return response.Success(HttpStatusCode.OK.GetHashCode(), userDto);
    }
    
    public async Task<ResultResponse<UserGetDto>> ConfirmAccountAsync(AuthConfirmCodeDto authConfirmCodeDto)
    {
        var response = new ResultResponse<UserGetDto>();
        var userResponse = await _validateCode(authConfirmCodeDto);
        if (!userResponse.IsSuccess)
        {
            var message = userResponse.Message ?? ErrorMessages.Auth.InvalidCode;
            return response.Fail(userResponse.StatusCode, message);
        }
        
        var userOnDb = userResponse.Data;
        userOnDb.ConfirmedAccount = true;
        userOnDb.ConfirmationCode = null;
        userOnDb.Modified = DateTime.Now;
        
        var userUpdate = await _userRepository.UpdateAsync(userOnDb);
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), _mapper.Map<UserGetDto>(userUpdate));
    }
    
    public async Task<ResultResponse<UserGetDto>> VerifyCode(AuthConfirmCodeDto authConfirmCodeDto)
    {
        var response = new ResultResponse<UserGetDto>();
        var userResponse = await _validateCode(authConfirmCodeDto);
       
        if (!userResponse.IsSuccess)
        {
            var message = userResponse.Message ?? ErrorMessages.Auth.InvalidCode;
            return response.Fail(userResponse.StatusCode, message);
        }

        var userOnDb = userResponse.Data;
        userOnDb.ConfirmationCode = null;
        userOnDb.Modified = DateTime.Now;
        
        await _userRepository.UpdateAsync(userOnDb);
        
        var token = _jwtService.GenerateJwtToken(userOnDb.Id, userOnDb.Email, userOnDb.FullName, userOnDb.IsAdmin, true, 1);

        var userDto = _mapper.Map<UserGetDto>(userOnDb);
        userDto.Token = token;
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), _mapper.Map<UserGetDto>(userDto));
    }
    
    public async Task<ResultResponse<bool>> ForgotPasswordAsync(string email)
    {
        var response = new ResultResponse<bool>();
        var userOnDb = await _userRepository.GetByEmailAsync(email);
        if (userOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Auth.UserNotFound);

        if (userOnDb.ConfirmationCode is not null)
            return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), ErrorMessages.Auth.UnconfirmedAccount);
        
        
        userOnDb.ConfirmationCode = _userService.GenerateRandomConfirmationCode();
        userOnDb.Modified = DateTime.Now;
        
        await _userRepository.UpdateAsync(userOnDb);
        
        var subject = "Recuperar sua senha";
        var messageMail = $@"
                        <p>Olá {userOnDb.FullName}, esta com problemas para acessar a AzAutoParkinkg?</p>
                        <p>Utilize o código abaixo para dar sequência.</p>
                        <p>Código de verificação: {userOnDb.ConfirmationCode}</p>
                        ";

        await _emailService.NotifyUserAsync(userOnDb.Email, subject, message: messageMail);
        return response.Success(HttpStatusCode.NoContent.GetHashCode(), true);
    }

    public async Task<ResultResponse<bool>> ResetPasswordAsync(AuthResetPasswordDto authResetPasswordDto)
    {
        var response = new ResultResponse<bool>();
        var userOnDb = await _userRepository.GetByIdAsync(authResetPasswordDto.Id);
        
        if (userOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Auth.UserNotFound);
        
        var passwordHasher = _hasherService.HashPassword(authResetPasswordDto.NewPassword);
        userOnDb.Password = passwordHasher;
        userOnDb.Modified = DateTime.Now;
        
        var userUpdated = await _userRepository.UpdateAsync(userOnDb);
        var userResponse = _mapper.Map<UserGetDto>(userUpdated);
        
        return response.Success(HttpStatusCode.NoContent.GetHashCode(), true);
    }

    public async Task<ResultResponse<UserGetDto>> ChangePasswordAsync(AuthChangePasswordDto authChangePasswordDto)
    {
        var response = new ResultResponse<UserGetDto>();
        var userOnDb = await _userRepository.GetByIdAsync(authChangePasswordDto.Id);
        if (userOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Auth.UserNotFound);
        
        
        var verifyPass = _hasherService.VerifyHashedPassword(authChangePasswordDto.OldPassword, userOnDb.Password);
        if (!verifyPass)
            return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), ErrorMessages.Auth.OldPasswordInvalid);
        
        var passwordHasher = _hasherService.HashPassword(authChangePasswordDto.NewPassword);
        userOnDb.Password = passwordHasher;
        userOnDb.Modified = DateTime.Now;
        
        var userUpdated = await _userRepository.UpdateAsync(userOnDb);
        var userResponse = _mapper.Map<UserGetDto>(userUpdated);
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), userResponse);
    }

    
    private async Task<ResultResponse<User>> _validateCode(AuthConfirmCodeDto authConfirmCode)
    {
        var response = new ResultResponse<User>();
        var userOnDb = await _userRepository.GetByEmailAsync(authConfirmCode.Email);
        if (userOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Auth.UserNotFound);
        

        if(userOnDb.ConfirmationCode != authConfirmCode.Code)
            return response.Fail(HttpStatusCode.BadRequest.GetHashCode(), ErrorMessages.Auth.InvalidCode);
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), userOnDb);
    }
}