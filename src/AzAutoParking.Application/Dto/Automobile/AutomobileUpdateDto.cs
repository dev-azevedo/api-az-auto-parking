namespace AzAutoParking.Application.Dto.Automobile;

public class AutomobileUpdateDto : AutomobileCreateDto
{
    public required long Id { get; set; }
}