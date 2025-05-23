namespace AzAutoParking.Application.Dto.Auth;

public class AuthSignUpDto
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmedPassword { get; set; }
}