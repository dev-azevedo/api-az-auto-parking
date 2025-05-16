namespace AzAutoParking.Application.Dto.User;

public class UserConfirmCodeDto
{
    public required string Email { get; set; }
    public required string Code { get; set; }
}