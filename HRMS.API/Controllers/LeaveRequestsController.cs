using HRMS.Application.Common.Models;
using HRMS.Application.LeaveRequests.Commands.ProcessLeaveRequest;
using HRMS.Application.LeaveRequests.Commands.SubmitLeaveRequest;
using HRMS.Application.LeaveRequests.Queries.GetLeaveRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class LeaveRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> SubmitLeaveRequest(SubmitLeaveRequestCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Success)
        {
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
}
