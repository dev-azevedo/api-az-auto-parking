namespace AzAutoParking.Application.Dto.User;

public class UserSignInDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}