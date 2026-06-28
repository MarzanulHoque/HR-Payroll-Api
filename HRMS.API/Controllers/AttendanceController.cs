using HRMS.Application.Attendance.Commands.ClockIn;
using HRMS.Application.Attendance.Commands.ClockOut;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using HRMS.API.Hubs;

namespace HRMS.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize] // Requires user to be logged in with JWT Token
public class AttendanceController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHubContext<NotificationsHub>? _hubContext;

    public AttendanceController(IMediator mediator, IHubContext<NotificationsHub>? hubContext = null)
    {
        _mediator = mediator;
        _hubContext = hubContext;
    }

    [HttpPost("clock-in")]
    public async Task<ActionResult<ApiResponse<Guid>>> ClockIn(ClockInCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Success)
        {
            await BroadcastDashboardUpdate("attendance.clock-in", result.Data);
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost("clock-out")]
    public async Task<ActionResult<ApiResponse<Guid>>> ClockOut(ClockOutCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Success)
        {
            await BroadcastDashboardUpdate("attendance.clock-out", result.Data);
            return Ok(result);
        }
        return BadRequest(result);
    }

    private async Task BroadcastDashboardUpdate(string source, Guid entityId)
    {
        if (_hubContext == null)
        {
            return;
        }

        try
        {
            await _hubContext.Clients.All.SendAsync("DashboardUpdated", new
            {
                Source = source,
                EntityId = entityId,
                Timestamp = DateTime.UtcNow
            });
        }
        catch
        {
            // best-effort only
        }
    }
}
