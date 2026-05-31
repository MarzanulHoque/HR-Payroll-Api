using System.Threading.Tasks;
using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Entities;

namespace HRMS.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly Data.ApplicationDbContext _db;
    private readonly IEmailService _emailService;

    public NotificationService(Data.ApplicationDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    public async Task CreateAsync(string title, string body, string recipient)
    {
        var n = new Notification
        {
            Title = title,
            Body = body,
            Recipient = recipient,
            IsRead = false
        };

        _db.Notifications.Add(n);
        await _db.SaveChangesAsync(default);

        // send email notification as well (noop in dev)
        await _emailService.SendEmailAsync(recipient, title, body);
    }
}
