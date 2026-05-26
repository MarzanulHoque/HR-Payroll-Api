using HRMS.Application.Common.Models;
using HRMS.Application.Employees.Commands.CreateEmployee;
using HRMS.Application.Employees.Commands.DeleteEmployee;
using HRMS.Application.Employees.Commands.UpdateEmployee;
using HRMS.Application.Employees.Queries.GetEmployeeById;
using HRMS.Application.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize] // Enforce JWT Authentication for Employee CRUD
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<EmployeeDto>>>> GetAll()
    {
        var result = await _mediator.Send(new GetEmployeesQuery());
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetEmployeeByIdQuery(id));
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> CreateEmployee([FromBody] CreateEmployeeCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Guid>>> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(ApiResponse<Guid>.FailureResponse("Mismatched Employee ID"));
        }

        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Guid>>> DeleteEmployee(Guid id)
    {
        var result = await _mediator.Send(new DeleteEmployeeCommand(id));
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
