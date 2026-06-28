using HRMS.Application.Common.Models;
using HRMS.Application.CompanyConfiguration.Commands.UpdateOrganizationPolicy;
using HRMS.Application.CompanyConfiguration.Queries.GetOrganizationPolicy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/v1/company-configuration")]
[Authorize(Roles = "Admin")]
public class CompanyConfigurationController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyConfigurationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("organization-policy")]
    public async Task<ActionResult<ApiResponse<OrganizationPolicyDto>>> GetOrganizationPolicy()
    {
        var result = await _mediator.Send(new GetOrganizationPolicyQuery());
        return Ok(result);
    }

    [HttpPut("organization-policy")]
    public async Task<ActionResult<ApiResponse<OrganizationPolicyDto>>> UpdateOrganizationPolicy([FromBody] UpdateOrganizationPolicyCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}