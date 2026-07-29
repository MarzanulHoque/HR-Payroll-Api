using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Attendance.Commands.ClockIn;

public class ClockInCommandHandler : IRequestHandler<ClockInCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public ClockInCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(ClockInCommand request, CancellationToken cancellationToken)
    {
        // To keep things robust, ensure the employee exists
        var employeeExists = await _context.Employees.AnyAsync(e => e.Id == request.EmployeeId, cancellationToken);
        if (!employeeExists)
        {
            return ApiResponse<Guid>.FailureResponse("Employee not found.", new List<string> { $"Cannot find Employee with Id {request.EmployeeId}" });
        }

        // Prevent double clocking in on the same day if they haven't cl ocked out
        var today = DateTime.UtcNow.Date;
        var openRecord = await _context.AttendanceRecords
            .FirstOrDefaultAsync(a => a.EmployeeId == request.EmployeeId 
                                      && a.Date == today 
                                      && a.ClockOutTime == null, cancellationToken);

        if (openRecord != null)
        {
            return ApiResponse<Guid>.FailureResponse("Already clocked in.", new List<string> { "You must clock out before clocking in again." });
        }

        var record = new AttendanceRecord
        {
            EmployeeId = request.EmployeeId,
            Date = today,
            ClockInTime = DateTime.UtcNow,
            Status = request.Status ?? "On Time"
        };

        _context.AttendanceRecords.Add(record);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(record.Id, "Clocked in successfully.");
    }
}
