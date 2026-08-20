using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace PersonalWebSite.EmailService;

public interface IEmailService
{
    Task SendContactMessageAsync(string fromName, string fromEmail, string message);
}

public class EmailSettings
{
    public string SmtpHost { get; set; } = "";
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; } = "";
    public string SmtpPass { get; set; } = "";
    public string ToAddress { get; set; } = "";
}

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendContactMessageAsync(string fromName, string fromEmail, string message)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(fromName, _settings.SmtpUser));
        email.To.Add(MailboxAddress.Parse(_settings.ToAddress));
        email.ReplyTo.Add(MailboxAddress.Parse(fromEmail));
        email.Subject = $"پیام جدید از {fromName}";
        email.Body = new TextPart("plain")
        {
            Text = $"از طرف: {fromName} <{fromEmail}>\n\n{message}"
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
