using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Designations.Commands.UpdateDesignation;

public record UpdateDesignationCommand(
    Guid Id,
    string Title,
    string? Description,
    Guid? DepartmentId,
    bool IsActive
) : IRequest<ApiResponse<Guid>>;
