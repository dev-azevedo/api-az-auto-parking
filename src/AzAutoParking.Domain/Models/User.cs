namespace AzAutoParking.Domain.Models;

public class User : BaseModel
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool ConfirmedAccount { get; set; } = false;
    public string? ConfirmationCode { get; set; }
    public bool IsAdmin { get; set; } = false;
    
    public List<ParkingSession>? ParkingSessions {get; set;}
    public List<Automobile>? Automobile {get; set;}
    public List<Log>? Log {get; set;}
}