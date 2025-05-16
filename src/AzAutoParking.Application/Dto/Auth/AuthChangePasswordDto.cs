namespace AzAutoParking.Application.Dto.Auth;

public class AuthChangePasswordDto
{
    public long Id { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
    public required string ConfirmedNewPassword { get; set; }
}