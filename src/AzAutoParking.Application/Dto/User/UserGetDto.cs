namespace AzAutoParking.Application.Dto.User;

public abstract record UserGetDto(long Id, string FullName, string Email, bool IsAdmin);