using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Attendance.Commands.ClockIn;

public record ClockInCommand(
    Guid EmployeeId,
    string? Status
) : IRequest<ApiResponse<Guid>>;
