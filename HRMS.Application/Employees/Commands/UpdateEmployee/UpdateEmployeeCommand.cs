using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Department,
    string Designation
) : IRequest<ApiResponse<Guid>>;
