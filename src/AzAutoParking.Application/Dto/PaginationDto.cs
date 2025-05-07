namespace AzAutoParking.Application.Dto;

public class PaginationDto<T>
{
    public List<T> Items { get; set; }
    public int TotalItems { get; set; }
}