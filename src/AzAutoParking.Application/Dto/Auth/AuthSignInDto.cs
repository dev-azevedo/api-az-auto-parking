namespace AzAutoParking.Application.Dto.Auth;

public class AuthSignInDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}