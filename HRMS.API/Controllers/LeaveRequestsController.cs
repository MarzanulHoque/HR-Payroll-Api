using HRMS.Application.Common.Models;
using HRMS.Application.LeaveRequests.Commands.ProcessLeaveRequest;
using HRMS.Application.LeaveRequests.Commands.SubmitLeaveRequest;
using HRMS.Application.LeaveRequests.Queries.GetLeaveRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using HRMS.API.Hubs;

namespace HRMS.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class LeaveRequestsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHubContext<NotificationsHub>? _hubContext;

    public LeaveRequestsController(IMediator mediator, IHubContext<NotificationsHub>? hubContext = null)
    {
        _mediator = mediator;
        _hubContext = hubContext;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> SubmitLeaveRequest(SubmitLeaveRequestCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Success)
        {
            await BroadcastDashboardUpdate("leave.submit", result.Data);
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPut("{id}/process")]
    public async Task<ActionResult<ApiResponse<bool>>> ProcessLeaveRequest(Guid id, [FromBody] string status)
    {
        var result = await _mediator.Send(new ProcessLeaveRequestCommand(id, status));
        if (result.Success)
        {
            await BroadcastDashboardUpdate($"leave.{status}", id);
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<LeaveRequestDto>>>> GetAllLeaveRequests()
    {
        var result = await _mediator.Send(new GetAllLeaveRequestsQuery());
        return Ok(result);
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
