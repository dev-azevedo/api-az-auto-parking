namespace AzAutoParking.Application.Dto.Auth;

public class AuthConfirmCodeDto
{
    public required string Email { get; set; }
    public required string Code { get; set; }
}