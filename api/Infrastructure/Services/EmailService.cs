using MailKit.Net.Smtp;
using MimeKit;

namespace api.Infrastructure.Services;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody, byte[] attachment = null, string attachmentName = null)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("YourApp", "yourapp@example.com"));
        emailMessage.To.Add(new MailboxAddress("", to));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };

        if (attachment != null && attachmentName != null)
        {
            bodyBuilder.Attachments.Add(attachmentName, attachment);
        }

        emailMessage.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            client.CheckCertificateRevocation = false;
            await client.ConnectAsync(_configuration["EmailSettings:Host"], int.Parse(_configuration["EmailSettings:Port"]), bool.Parse(_configuration["EmailSettings:UseSsl"]));
            await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
