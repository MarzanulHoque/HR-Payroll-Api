using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.LeaveRequests.Commands.SubmitLeaveRequest;

public record SubmitLeaveRequestCommand(
    Guid EmployeeId,
    string LeaveType,
    DateTime StartDate,
    DateTime EndDate,
    string? Reason
) : IRequest<ApiResponse<Guid>>;
