namespace AzAutoParking.Domain.Models;


public class Parking : BaseModel
{
    public required int ParkingNumber {get; set;}
    public required bool Available { get; set; } = true;
    
    public List<Reservation>? Reservations {get; set;}
}