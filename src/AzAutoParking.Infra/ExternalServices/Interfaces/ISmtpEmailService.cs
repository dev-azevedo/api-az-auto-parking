namespace AzAutoParking.Infra.ExternalServices.Interfaces;

public interface ISmtpEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}