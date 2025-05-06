namespace AzAutoParking.Domain.Models;

public class Reservation : BaseModel
{
    public required long AutomobileId {get; set;}
    public required long ParkingId {get; set;}
    public required long UserId {get; set;}
    public Decimal? Price {get; set;}
    public bool Finished {get; set;} = false;
    public DateTime? FinishedAt {get; set;}
    
    public required Automobile Automobile {get; set;}
    public required Parking Parking {get; set;}
    public required User User {get; set;}
}