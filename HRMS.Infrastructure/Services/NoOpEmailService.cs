using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;

namespace HRMS.Infrastructure.Services;

public class NoOpEmailService : IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body, IEnumerable<EmailAttachment>? attachments = null, CancellationToken cancellationToken = default)
    {
        // Intentionally do nothing. Safe for tests and local development.
        Console.WriteLine($"[NoOpEmailService] Skipping send. To={to}, Subject={subject}");
        return Task.CompletedTask;
    }
}
