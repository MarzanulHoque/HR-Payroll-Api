using HRMS.Application.Common.Models;
using HRMS.Application.Employees.Queries.GetEmployees;
using MediatR;

namespace HRMS.Application.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(Guid Id) : IRequest<ApiResponse<EmployeeDto>>;
