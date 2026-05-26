namespace HRMS.Application.Designations.Queries.GetDesignationById;

public record DesignationDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid? DepartmentId { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}
