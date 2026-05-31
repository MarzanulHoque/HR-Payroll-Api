using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace HRMS.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public SmtpEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body, IEnumerable<EmailAttachment>? attachments = null, CancellationToken cancellationToken = default)
    {
        var host = _configuration["Smtp:Host"];
        var port = int.TryParse(_configuration["Smtp:Port"], out var p) ? p : 25;
        var username = _configuration["Smtp:Username"];
        var password = _configuration["Smtp:Password"];
        var from = _configuration["Smtp:From"] ?? username;

        // If SMTP host not configured, fallback to no-op (useful for tests/local)
        if (string.IsNullOrWhiteSpace(host))
        {
            Console.WriteLine($"[SmtpEmailService] Skipping send; SMTP not configured. To={to}, Subject={subject}");
            return;
        }

        using var msg = new MailMessage();
        msg.From = new MailAddress(from!);
        msg.To.Add(new MailAddress(to));
        msg.Subject = subject;
        msg.Body = body;
        msg.IsBodyHtml = false;

        if (attachments != null)
        {
            foreach (var a in attachments)
            {
                var stream = new System.IO.MemoryStream(a.Content);
                var attach = new Attachment(stream, a.FileName, a.ContentType);
                msg.Attachments.Add(attach);
            }
        }

        using var client = new SmtpClient(host, port);
        if (!string.IsNullOrWhiteSpace(username))
        {
            client.Credentials = new NetworkCredential(username, password);
            client.EnableSsl = true;
        }

        await client.SendMailAsync(msg, cancellationToken);
    }
}
