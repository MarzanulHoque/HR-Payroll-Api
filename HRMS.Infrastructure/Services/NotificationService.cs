using System.Threading.Tasks;
using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Entities;
using HRMS.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
// SignalR broadcasting is handled in the API layer to avoid coupling Infrastructure -> API

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


    public async Task<List<NotificationDto>> GetForRecipientAsync(string recipient, int page = 1, int pageSize = 25)
    {
        var q = _db.Notifications
            .Where(n => n.Recipient == recipient)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var list = await q.Select(n => new NotificationDto(n.Id, n.Title, n.Body, n.Recipient, n.IsRead, n.CreatedAt)).ToListAsync();
        return list;
    }

    public async Task<bool> MarkAsReadAsync(Guid id, string recipient)
    {
        var n = await _db.Notifications.FirstOrDefaultAsync(x => x.Id == id && x.Recipient == recipient);
        if (n == null) return false;
        n.IsRead = true;
        await _db.SaveChangesAsync(default);
        return true;
    }
}
