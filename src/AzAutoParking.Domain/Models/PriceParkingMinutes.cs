namespace AzAutoParking.Domain.Models;

public class PriceParkingMinutes : BaseModel
{
    public required int Minutes {get; set;}
    public required Decimal Price {get; set;}
}