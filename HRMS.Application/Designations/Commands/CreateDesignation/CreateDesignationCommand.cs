using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Designations.Commands.CreateDesignation;

public record CreateDesignationCommand(
    string Title,
    string? Description,
    Guid? DepartmentId,
    bool IsActive = true
) : IRequest<ApiResponse<Guid>>;
