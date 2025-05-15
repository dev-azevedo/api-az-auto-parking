using AzAutoParking.Application.Interfaces;
using AzAutoParking.Domain.Interfaces;

namespace AzAutoParking.Application.Services;

public class EmailService(ISmtpEmailService smtpEmailService) : IEmailService
{
    private readonly ISmtpEmailService _smtpEmailService = smtpEmailService;

    public async Task NotifyUserAsync(string email, string subject, string message)
    {
        await _smtpEmailService.SendEmailAsync(email, subject,message);
    }
}