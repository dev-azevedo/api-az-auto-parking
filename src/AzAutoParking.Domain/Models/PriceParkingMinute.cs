namespace AzAutoParking.Domain.Models;

public class PriceParkingMinute : BaseModel
{
    public required int Minutes {get; set;}
    public required Decimal Price {get; set;}
}