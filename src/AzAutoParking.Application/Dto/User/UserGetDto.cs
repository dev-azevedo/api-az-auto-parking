namespace AzAutoParking.Application.Dto.User;

public record UserGetDto(long Id, string FullName, string Email, bool IsAdmin);