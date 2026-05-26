using HRMS.Application.Common.Models;
using HRMS.Application.Departments.Queries.GetDepartments;
using MediatR;

namespace HRMS.Application.Departments.Queries.GetDepartmentById;

public record GetDepartmentByIdQuery(Guid Id) : IRequest<ApiResponse<DepartmentDto>>;
