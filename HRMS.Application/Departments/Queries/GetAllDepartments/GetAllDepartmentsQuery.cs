using HRMS.Application.Common.Models;
using HRMS.Application.Departments.Queries.GetDepartmentById;
using MediatR;
using System.Collections.Generic;

namespace HRMS.Application.Departments.Queries.GetAllDepartments;

public record GetAllDepartmentsQuery : IRequest<ApiResponse<List<DepartmentDto>>>;
