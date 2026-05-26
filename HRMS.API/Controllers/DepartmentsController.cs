using HRMS.Application.Common.Models;
using HRMS.Application.Departments.Commands.CreateDepartment;
using HRMS.Application.Departments.Commands.DeleteDepartment;
using HRMS.Application.Departments.Commands.UpdateDepartment;
using HRMS.Application.Departments.Queries.GetAllDepartments;
using HRMS.Application.Departments.Queries.GetDepartmentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> CreateDepartment(CreateDepartmentCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DepartmentDto>>>> GetAllDepartments()
    {
        var result = await _mediator.Send(new GetAllDepartmentsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DepartmentDto>>> GetDepartmentById(Guid id)
    {
        var result = await _mediator.Send(new GetDepartmentByIdQuery(id));
        if (result.Success)
        {
            return Ok(result);
        }
        return NotFound(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Guid>>> UpdateDepartment(Guid id, UpdateDepartmentCommand command)
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
    public async Task<ActionResult<ApiResponse<Guid>>> DeleteDepartment(Guid id)
    {
        var result = await _mediator.Send(new DeleteDepartmentCommand(id));
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
