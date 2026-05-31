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
}

public record CreateNotificationRequest(string Title, string Body, string Recipient);
