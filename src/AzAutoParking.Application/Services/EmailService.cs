using AzAutoParking.Application.Interfaces;
using AzAutoParking.Domain.Interfaces;

namespace AzAutoParking.Application.Services;

public class EmailService(ISmtpEmailService smtpEmailService) : IEmailService
{
    private readonly ISmtpEmailService _smtpEmailService = smtpEmailService;

    public async Task NotifyUserAsync(string email, string subject, string messageHtml)
    {
        var bodyMessage = BodyMessageHtml();
        bodyMessage = bodyMessage.Replace("{{MensagemPersonalizada}}", messageHtml);
        await _smtpEmailService.SendEmailAsync(email, subject,bodyMessage);
    }

    private static string BodyMessageHtml()
    {
        var bodyHtml =
            "<!DOCTYPE html>\n<html lang=\"pt-BR\">\n  <head>\n    <meta charset=\"UTF-8\" />\n    <title>Notificação AzAutoParking</title>\n  </head>\n  <body style=\"font-family: Arial, sans-serif; background-color: #f7f7f7; padding: 20px;\">\n    <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\">\n      <tr style=\"background-color: #005baa;\">\n        <td style=\"padding: 20px; color: #ffffff; text-align: center;\">\n          <h1 style=\"margin: 0;\">AzAutoParking</h1>\n        </td>\n      </tr>\n      <tr>\n        <td style=\"padding: 30px;\">\n          <p style=\"font-size: 16px; color: #333333;\">\n            Este é um e-mail de notificação automático enviado pelo sistema <strong>AzAutoParking</strong>.\n          </p>\n          <p style=\"font-size: 16px; color: #333333;\">\n            {{MensagemPersonalizada}}\n          </p>\n          <p style=\"font-size: 16px; color: #333333;\">\n            Se tiver dúvidas, entre em contato com nosso suporte.\n          </p>\n          <p style=\"font-size: 16px; color: #333333;\">Atenciosamente,<br /><strong>Equipe AzAutoParking</strong></p>\n        </td>\n      </tr>\n      <tr>\n        <td style=\"background-color: #f0f0f0; text-align: center; padding: 15px; font-size: 12px; color: #999999;\">\n          © 2025 AzAutoParking. Todos os direitos reservados.\n        </td>\n      </tr>\n    </table>\n  </body>\n</html>";
        return bodyHtml;
    }
}