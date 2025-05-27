using System.Net;
using AzAutoParking.Application.Dto.Auth;
using AzAutoParking.Application.Dto.User;
using AzAutoParking.Application.Interfaces;
using AzAutoParking.Application.Response;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using Mapster;

namespace AzAutoParking.Application.Services;

public class AuthService(IJwtService jwtService, IUserRepository userRepository, IUserService userService, IEmailService emailService) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IUserService _userService = userService;
    private readonly IEmailService _emailService = emailService;
    private readonly IPasswordHasherService _hasherService = new PasswordHasherService();
    
    
    public async Task<ResultResponse<UserGetDto>> SignInAsync(AuthSignInDto authSignInDto)
    {
        var response = new ResultResponse<UserGetDto>();
        var userOnDb = await _userRepository.GetByEmailAsync(authSignInDto.Email);
        if (userOnDb is null)
            return response.Fail(HttpStatusCode.BadRequest.GetHashCode(), ErrorMessages.Auth.InvalidEmailOrPassword);
        
        var verifyPass = _hasherService.VerifyHashedPassword(authSignInDto.Password, userOnDb.Password);
        if (!verifyPass)
            return response.Fail(HttpStatusCode.Unauthorized.GetHashCode(), ErrorMessages.Auth.InvalidEmailOrPassword);

        if(!userOnDb.ConfirmedAccount)
            return response.Fail(HttpStatusCode.Forbidden.GetHashCode(), ErrorMessages.Auth.UnconfirmedAccount);


        var token = _jwtService.GenerateJwtToken(userOnDb.Id, userOnDb.Email, userOnDb.FullName, userOnDb.IsAdmin);

        var userDto = userOnDb.Adapt<UserGetDto>();
        userDto.Token = token;

        return response.Success(HttpStatusCode.OK.GetHashCode(), userDto);
    }

    public async Task<ResultResponse<UserGetDto>> SignUpAsync(AuthSignUpDto user)
    {
        var response = new ResultResponse<UserGetDto>();
        var userOnDb = await _userRepository.GetByEmailAsync(user.Email);

        if (userOnDb is not null)
            return response.Fail(HttpStatusCode.Conflict.GetHashCode(), ErrorMessages.Auth.EmailAlreadyExists);

        var passwordHasher = _hasherService.HashPassword(user.Password);
        var userModel = user.Adapt<User>();
        userModel.Password = passwordHasher;
        userModel.ConfirmationCode = _userService.GenerateRandomConfirmationCode();
        
        var userCreated = await _userRepository.CreateAsync(userModel);
        var userDto = userCreated.Adapt<UserGetDto>();

        var subject = "Bem-vindo ao AzAutoParking";
        var message = $@"
                        <p>Olá {userModel.FullName}, seja bem-vindo(a)</p>
                        <p>Utilize esse código para verificar sua conta: {userModel.ConfirmationCode}</p>
                        ";

        await _emailService.NotifyUserAsync(user.Email, subject, message: message);

        return response.Success(
            HttpStatusCode.Created.GetHashCode(),
            userDto);
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
        userOnDb.IsModified();
        
        var userUpdate = await _userRepository.UpdateAsync(userOnDb);
        
        var token = _jwtService.GenerateJwtToken(userOnDb.Id, userOnDb.Email, userOnDb.FullName, userOnDb.IsAdmin);

        var userDto = userUpdate.Adapt<UserGetDto>();
        userDto.Token = token;
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), userDto);
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
        userOnDb.IsModified();
        
        await _userRepository.UpdateAsync(userOnDb);
        
        var token = _jwtService.GenerateJwtToken(userOnDb.Id, userOnDb.Email, userOnDb.FullName, userOnDb.IsAdmin, true, 1);

        var userDto = userOnDb.Adapt<UserGetDto>();
        userDto.Token = token;
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), userDto);
    }
    
    public async Task<ResultResponse<bool>> ForgotPasswordAsync(string email)
    {
        var response = new ResultResponse<bool>();
        var userOnDb = await _userRepository.GetByEmailAsync(email);
        if (userOnDb is null)
            return response.Fail(HttpStatusCode.NotFound.GetHashCode(), ErrorMessages.Auth.UserNotFound);
        
        userOnDb.ConfirmationCode = _userService.GenerateRandomConfirmationCode();
        userOnDb.IsModified();
        
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
        userOnDb.IsModified();
        
        await _userRepository.UpdateAsync(userOnDb);
        
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
        userOnDb.IsModified();
        
        var userUpdated = await _userRepository.UpdateAsync(userOnDb);
        var userDto = userUpdated.Adapt<UserGetDto>();
        
        return response.Success(HttpStatusCode.OK.GetHashCode(), userDto);
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