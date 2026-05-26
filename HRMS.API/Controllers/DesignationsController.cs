using HRMS.Application.Common.Models;
using HRMS.Application.Designations.Commands.CreateDesignation;
using HRMS.Application.Designations.Commands.DeleteDesignation;
using HRMS.Application.Designations.Commands.UpdateDesignation;
using HRMS.Application.Designations.Queries.GetAllDesignations;
using HRMS.Application.Designations.Queries.GetDesignationById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class DesignationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DesignationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> CreateDesignation(CreateDesignationCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DesignationDto>>>> GetAllDesignations()
    {
        var result = await _mediator.Send(new GetAllDesignationsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DesignationDto>>> GetDesignationById(Guid id)
    {
        var result = await _mediator.Send(new GetDesignationByIdQuery(id));
        if (result.Success)
        {
            return Ok(result);
        }
        return NotFound(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Guid>>> UpdateDesignation(Guid id, UpdateDesignationCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(ApiResponse<Guid>.FailureResponse("Id mismatch."));
        }

        var result = await _mediator.Send(command);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Guid>>> DeleteDesignation(Guid id)
    {
        var result = await _mediator.Send(new DeleteDesignationCommand(id));
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
