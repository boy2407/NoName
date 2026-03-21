using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NoName.Application.Abstractions.Services;
using NoName.Infrastructure.Settings;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeKit.MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Mail));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = htmlMessage };
        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.Mail, _emailSettings.Password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}