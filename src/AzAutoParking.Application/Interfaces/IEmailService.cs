namespace AzAutoParking.Application.Interfaces;

public interface IEmailService
{
    Task NotifyUserAsync(string email, string subject, string message);
}