using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Departments.Queries.GetDepartmentTree;

public record GetDepartmentsTreeQuery() : IRequest<ApiResponse<List<DepartmentTreeDto>>>;
