namespace AzAutoParking.Domain.Models;

public class BaseModel
{
    public long Id { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Modified { get; set; } = DateTime.Now;
    
    public void Deactivate()
    {
        IsActive = false;
    }

    public void IsModified()
    {
        Modified = DateTime.Now;   
    }
}