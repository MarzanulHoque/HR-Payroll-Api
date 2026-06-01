using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Dashboard.Queries.GetAttendanceTrends;

public class GetAttendanceTrendsQueryHandler : IRequestHandler<GetAttendanceTrendsQuery, ApiResponse<List<AttendanceTrendPointDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetAttendanceTrendsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<AttendanceTrendPointDto>>> Handle(GetAttendanceTrendsQuery request, CancellationToken cancellationToken)
    {
        var referenceDate = ParseMonth(request.Month) ?? DateTime.UtcNow.Date;
        var monthStart = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        var nextMonth = monthStart.AddMonths(1);

        var counts = await _context.AttendanceRecords
            .Where(a => a.Date >= monthStart && a.Date < nextMonth)
            .GroupBy(a => a.Date.Day)
            .Select(g => new { Day = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var daysInMonth = DateTime.DaysInMonth(monthStart.Year, monthStart.Month);
        var points = Enumerable.Range(1, daysInMonth)
            .Select(day => new AttendanceTrendPointDto
            {
                Label = day.ToString(),
                Value = counts.FirstOrDefault(x => x.Day == day)?.Count ?? 0
            })
            .ToList();

        return ApiResponse<List<AttendanceTrendPointDto>>.SuccessResponse(points);
    }

    private static DateTime? ParseMonth(string? month)
    {
        if (string.IsNullOrWhiteSpace(month))
        {
            return null;
        }

        if (DateTime.TryParseExact(month, "MMMM yyyy", null, System.Globalization.DateTimeStyles.None, out var pretty))
        {
            return pretty;
        }

        if (DateTime.TryParseExact(month, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out var iso))
        {
            return iso;
        }

        return DateTime.TryParse(month, out var parsed) ? parsed : null;
    }
}
