namespace HRMS.Application.Dashboard.Queries.GetAttendanceTrends;

public record AttendanceTrendPointDto
{
    public string Label { get; init; } = string.Empty;
    public int Value { get; init; }
}
