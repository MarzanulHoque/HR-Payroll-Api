using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;

namespace HRMS.Application.Employees.Commands.CreateEmployee;

/// <summary>
/// Command models the incoming request to create a new Employee.
/// It implements IRequest, returning out unified ApiResponse wrapping the new Employee ID.
/// </summary>
public record CreateEmployeeCommand(
    string FirstName, 
    string LastName, 
    string Email, 
    string Department, 
    string Designation
) : IRequest<ApiResponse<Guid>>;
