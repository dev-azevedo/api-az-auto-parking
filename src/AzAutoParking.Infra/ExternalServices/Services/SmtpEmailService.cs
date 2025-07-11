using System.Net;
using System.Net.Mail;
using AzAutoParking.Infra.ExternalServices.Interfaces;

namespace AzAutoParking.Infra.ExternalServices.Services;

public class SmtpEmailService(string smtpHost, int smtpPort, string from, string nameFrom, string password)
    : ISmtpEmailService
{
    private readonly SmtpClient _smtpClient = new(smtpHost)
    {
        Port = smtpPort,
        Credentials = new NetworkCredential(from, password),
        EnableSsl = true
    };
    private readonly string _from = from;

    public async Task SendEmailAsync(string to, string subject, string body)
    {


        var mail = new MailMessage
        {
            From = new MailAddress(_from, nameFrom),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        
        mail.To.Add(to);
        
        await _smtpClient.SendMailAsync(mail);
    }

}

