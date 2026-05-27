namespace HRMS.Application.Dashboard.Queries.GetDashboardMetrics;

public record DashboardMetricsDto
{
    public int TotalEmployees { get; init; }
    public int PendingLeaveRequests { get; init; }
    public int TotalAttendanceToday { get; init; }
}
