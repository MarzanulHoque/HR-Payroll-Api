using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.LeaveRequests.Commands.ProcessLeaveRequest;

public record ProcessLeaveRequestCommand(
    Guid LeaveRequestId,
    string Status // "Approved" or "Rejected"
) : IRequest<ApiResponse<bool>>;
