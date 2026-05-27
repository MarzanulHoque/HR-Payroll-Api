using HRMS.Application.Common.Models;
using HRMS.Application.Dashboard.Queries.GetDashboardMetrics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("metrics")]
    public async Task<ActionResult<ApiResponse<DashboardMetricsDto>>> GetMetrics()
    {
        var result = await _mediator.Send(new GetDashboardMetricsQuery());
        return Ok(result);
    }
}
