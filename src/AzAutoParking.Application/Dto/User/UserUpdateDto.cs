namespace AzAutoParking.Application.Dto.User;

public record UserUpdateDto(
    long Id, 
    string FullName, 
    string Email, 
    bool IsAdmin, 
    string? Password, 
    string? ConfirmedPassword);