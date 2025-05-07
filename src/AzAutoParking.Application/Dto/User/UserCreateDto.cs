namespace AzAutoParking.Application.Dto.User;

public abstract record UserCreateDto(string FullName, string Email, string Password, string ConfirmedPassword);