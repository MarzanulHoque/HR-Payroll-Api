using HRMS.Application.Attendance.Commands.ClockIn;
using HRMS.Application.Attendance.Commands.ClockOut;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize] // Requires user to be logged in with JWT Token
public class AttendanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttendanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("clock-in")]
    public async Task<ActionResult<ApiResponse<Guid>>> ClockIn(ClockInCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Success)
        {
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
            return Ok(result);
        }
        return BadRequest(result);
    }
}
