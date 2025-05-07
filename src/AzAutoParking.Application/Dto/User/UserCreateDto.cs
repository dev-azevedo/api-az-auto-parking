namespace AzAutoParking.Application.Dto.User;

public record UserCreateDto(string FullName, string Email, string Password, string ConfirmedPassword);