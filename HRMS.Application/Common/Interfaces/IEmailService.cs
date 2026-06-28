using HRMS.Application.Common.Models;

namespace HRMS.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, IEnumerable<EmailAttachment>? attachments = null, CancellationToken cancellationToken = default);
}
