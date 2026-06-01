namespace HRMS.Application.Dashboard.Queries.GetDepartmentDistribution;

public record DepartmentDistributionPointDto
{
    public string Label { get; init; } = string.Empty;
    public int Value { get; init; }
}
