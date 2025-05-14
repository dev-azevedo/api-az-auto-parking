namespace AzAutoParking.Application.Dto.User;

public class UserUpdateDto
{
    public long Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmedPassword { get; set; }
    public bool? IsAdmin { get; set; }
};