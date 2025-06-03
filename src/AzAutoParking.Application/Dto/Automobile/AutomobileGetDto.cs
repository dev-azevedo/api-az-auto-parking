using AzAutoParking.Domain.Enums;

namespace AzAutoParking.Application.Dto.Automobile;

public class AutomobileGetDto
{
    public required string Brand {get; set;}
    public required string Model {get; set;}
    public required string Color {get; set;}
    public required string Plate {get; set;}
    public required ETypeAutomobile TypeAutomobile {get; set;}
    public string? Client {get; set;} 
    public string? Contact {get; set;}
    public required long UserId { get; set; }
}