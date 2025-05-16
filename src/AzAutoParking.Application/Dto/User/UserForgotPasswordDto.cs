namespace AzAutoParking.Application.Dto.User;

public class UserForgotPasswordDto
{
    public required string Email { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmedNewPassword { get; set; }
}