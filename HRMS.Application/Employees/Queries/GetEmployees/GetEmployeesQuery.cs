using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Employees.Queries.GetEmployees;

/// <summary>
/// Query for retrieving employees with basic pagination.
/// Search/sort will be added in subsequent changes.
/// </summary>
public record GetEmployeesQuery(int Page = 1, int PageSize = 25) : IRequest<ApiResponse<List<EmployeeDto>>>;
