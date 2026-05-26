using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Employees.Queries.GetEmployees;

public record GetEmployeesQuery() : IRequest<ApiResponse<List<EmployeeDto>>>;
