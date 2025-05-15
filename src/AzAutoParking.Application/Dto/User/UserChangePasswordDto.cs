namespace AzAutoParking.Application.Dto.User;

public class UserChangePasswordDto
{
    public long Id { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
    public required string ConfirmedNewPassword { get; set; }
}