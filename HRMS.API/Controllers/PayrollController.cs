using HRMS.Application.Common.Models;
using HRMS.Application.Payroll.Commands.GeneratePayroll;
using HRMS.Application.Payroll.Queries.GetSalarySlips;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class PayrollController : ControllerBase
{
    private readonly IMediator _mediator;

    public PayrollController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ApiResponse<Guid>>> GeneratePayroll(GeneratePayrollCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("slips")]
    public async Task<ActionResult<ApiResponse<List<SalarySlipDto>>>> GetAllSalarySlips()
    {
        var result = await _mediator.Send(new GetAllSalarySlipsQuery());
        return Ok(result);
    }
}
