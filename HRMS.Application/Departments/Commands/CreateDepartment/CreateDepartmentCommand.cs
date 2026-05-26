using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(
    string Name,
    string? Description,
    Guid? ParentDepartmentId,
    Guid? ManagerId
) : IRequest<ApiResponse<Guid>>;
