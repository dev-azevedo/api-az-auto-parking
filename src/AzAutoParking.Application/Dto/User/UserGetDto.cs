namespace AzAutoParking.Application.Dto.User;

public class UserGetDto
{
    public required long Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required bool IsAdmin { get; set; }
    public string? Token { get; set; }
};