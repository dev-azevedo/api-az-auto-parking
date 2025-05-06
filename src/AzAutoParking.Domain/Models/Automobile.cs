namespace AzAutoParking.Domain.Models;

public class Automobile : BaseModel
{
    public required string Brand {get; set;}
    public required string Model {get; set;}
    public required string Color {get; set;}
    public required string Plate {get; set;}
    public string? Client {get; set;} 
    public string? Contact {get; set;}
    public required long UserId { get; set; }
    
    public required User User { get; set; }
    public List<Reservation>? Reservations {get; set;}
}