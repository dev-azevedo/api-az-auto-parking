namespace AzAutoParking.Application.Dto.Auth;

public class AuthResetPasswordDto
{
    public long Id { get; set; }
    public required string NewPassword { get; set; }
    public required string ConfirmedNewPassword { get; set; }
}