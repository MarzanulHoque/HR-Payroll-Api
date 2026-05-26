using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Attendance.Commands.ClockOut;

public class ClockOutCommandHandler : IRequestHandler<ClockOutCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public ClockOutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(ClockOutCommand request, CancellationToken cancellationToken)
    {
        // Find the active clock-in for today
        var today = DateTime.UtcNow.Date;
        var activeRecord = await _context.AttendanceRecords
            .FirstOrDefaultAsync(a => a.EmployeeId == request.EmployeeId 
                                      && a.Date == today 
                                      && a.ClockOutTime == null, cancellationToken);

        if (activeRecord == null)
        {
            return ApiResponse<Guid>.FailureResponse("No active clock-in found.", new List<string> { "You haven't clocked in today, or you have already clocked out." });
        }

        activeRecord.ClockOutTime = DateTime.UtcNow;
        activeRecord.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(activeRecord.Id, "Clocked out successfully.");
    }
}
