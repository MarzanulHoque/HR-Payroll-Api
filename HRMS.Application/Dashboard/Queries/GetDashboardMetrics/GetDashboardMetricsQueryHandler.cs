using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Dashboard.Queries.GetDashboardMetrics;

public class GetDashboardMetricsQueryHandler : IRequestHandler<GetDashboardMetricsQuery, ApiResponse<DashboardMetricsDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardMetricsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<DashboardMetricsDto>> Handle(GetDashboardMetricsQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var monthStart = new DateTime(today.Year, today.Month, 1);
        var monthEnd = monthStart.AddMonths(1);

        var totalEmployees = await _context.Employees.CountAsync(cancellationToken);
        
        var pendingLeaves = await _context.LeaveRequests
            .CountAsync(lr => lr.Status == "Pending", cancellationToken);

        var attendanceToday = await _context.AttendanceRecords
            .CountAsync(a => a.Date == today, cancellationToken);

        var payrollExpense = 0m;
        if (_context.SalarySlips != null)
        {
            payrollExpense = await _context.SalarySlips
                .Where(s => s.CreatedAt >= monthStart && s.CreatedAt < monthEnd)
                .SumAsync(s => (decimal?)s.NetPay, cancellationToken) ?? 0m;
        }

        var metrics = new DashboardMetricsDto
        {
            TotalEmployees = totalEmployees,
            PendingLeaveRequests = pendingLeaves,
            TotalAttendanceToday = attendanceToday,
            PayrollExpense = payrollExpense
        };

        return ApiResponse<DashboardMetricsDto>.SuccessResponse(metrics);
    }
}
