using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.LeaveRequests.Commands.ProcessLeaveRequest;

public class ProcessLeaveRequestCommandHandler : IRequestHandler<ProcessLeaveRequestCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public ProcessLeaveRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(ProcessLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _context.LeaveRequests.FindAsync(new object[] { request.LeaveRequestId }, cancellationToken);

        if (leaveRequest == null)
        {
            return ApiResponse<bool>.FailureResponse("Leave request not found.");
        }

        var validStatuses = new[] { "Approved", "Rejected" };
        if (!validStatuses.Contains(request.Status))
        {
            return ApiResponse<bool>.FailureResponse("Invalid status. Must be 'Approved' or 'Rejected'.");
        }

        leaveRequest.Status = request.Status;
        leaveRequest.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.SuccessResponse(true, $"Leave request {request.Status.ToLower()} successfully.");
    }
}
