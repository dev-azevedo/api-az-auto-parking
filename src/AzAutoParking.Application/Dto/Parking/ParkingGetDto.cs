namespace AzAutoParking.Application.Dto.Parking;

public class ParkingGetDto
{
    public long Id { get; set; }
    public long ParkingNumber { get; set; }
    public bool Available { get; set; }
}