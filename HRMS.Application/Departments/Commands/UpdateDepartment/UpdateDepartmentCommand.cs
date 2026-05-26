using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Departments.Commands.UpdateDepartment;

public record UpdateDepartmentCommand(
    Guid Id,
    string Name,
    string? Description,
    Guid? ParentDepartmentId,
    Guid? ManagerId
) : IRequest<ApiResponse<Guid>>;
