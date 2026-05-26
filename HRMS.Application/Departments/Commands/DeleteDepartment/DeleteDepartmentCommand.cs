using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Departments.Commands.DeleteDepartment;

public record DeleteDepartmentCommand(Guid Id) : IRequest<ApiResponse<Guid>>;
