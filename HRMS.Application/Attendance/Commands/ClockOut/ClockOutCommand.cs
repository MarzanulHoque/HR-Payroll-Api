using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Attendance.Commands.ClockOut;

public record ClockOutCommand(
    Guid EmployeeId
) : IRequest<ApiResponse<Guid>>;
