using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Departments.Queries.GetDepartments;

public record GetDepartmentsQuery() : IRequest<ApiResponse<List<DepartmentDto>>>;
