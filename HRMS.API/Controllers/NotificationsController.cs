using System.Threading.Tasks;
using HRMS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/v1/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest req)
    {
        await _notificationService.CreateAsync(req.Title, req.Body, req.Recipient);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<NotificationDto>>> GetForRecipient([FromQuery] string recipient, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
    {
        var items = await _notificationService.GetForRecipientAsync(recipient, page, pageSize);
        return Ok(items);
    }

    [HttpPut("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead([FromRoute] Guid id, [FromQuery] string recipient)
    {
        var ok = await _notificationService.MarkAsReadAsync(id, recipient);
        if (!ok) return NotFound();
        return NoContent();
    }
}

public record CreateNotificationRequest(string Title, string Body, string Recipient);

public record NotificationDto(Guid Id, string Title, string Body, string Recipient, bool IsRead, DateTime CreatedAt);
