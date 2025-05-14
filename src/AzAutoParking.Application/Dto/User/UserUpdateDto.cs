namespace AzAutoParking.Application.Dto.User;

public class UserUpdateDto
{
    public long Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmedPassword { get; set; }
    public required bool IsAdmin { get; set; }
};