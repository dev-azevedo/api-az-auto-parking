using AzAutoParking.Domain.Enums;

namespace AzAutoParking.Domain.Models;

public class Log :  BaseModel
{
    public required ENivelLog Nivel {get; set;}
    public required string Messagem {get; set;}
    public required long UserId {get; set;}
    public required string Endpoint {get; set;}
    public required int StatusCode {get; set;}
    
    public required User User {get; set;}
}