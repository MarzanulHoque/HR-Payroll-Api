using System.Threading.Tasks;
using HRMS.Application.Common.Interfaces;
using HRMS.API.Authorization;
using HRMS.Application.Common.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/v1/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly Microsoft.AspNetCore.SignalR.IHubContext<HRMS.API.Hubs.NotificationsHub>? _hubContext;

    public NotificationsController(INotificationService notificationService, Microsoft.AspNetCore.SignalR.IHubContext<HRMS.API.Hubs.NotificationsHub>? hubContext = null)
    {
        _notificationService = notificationService;
        _hubContext = hubContext;
    }

    [HttpPost]
    [PermissionAuthorize("notification.send")]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest req)
    {
        await _notificationService.CreateAsync(req.Title, req.Body, req.Recipient);

        // broadcast via SignalR to recipient if hub is available
        try
        {
            var dto = new NotificationDto(Guid.NewGuid(), req.Title, req.Body, req.Recipient, false, DateTime.UtcNow);
            if (_hubContext != null)
            {
                await _hubContext.Clients.User(req.Recipient).SendAsync("ReceiveNotification", dto);
            }
        }
        catch
        {
            // swallow - best-effort
        }

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<NotificationDto>>> GetForRecipient([FromQuery] int page = 1, [FromQuery] int pageSize = 25)
    {
        var recipient = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(recipient)) return Forbid();

        var items = await _notificationService.GetForRecipientAsync(recipient, page, pageSize);
        return Ok(items);
    }

    [HttpPut("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead([FromRoute] Guid id)
    {
        var recipient = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(recipient)) return Forbid();

        var ok = await _notificationService.MarkAsReadAsync(id, recipient);
        if (!ok) return NotFound();
        return NoContent();
    }
}

public record CreateNotificationRequest(string Title, string Body, string Recipient);
