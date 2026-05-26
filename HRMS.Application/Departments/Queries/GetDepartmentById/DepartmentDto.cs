using System;

namespace HRMS.Application.Departments.Queries.GetDepartmentById;

public record DepartmentDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid? ParentDepartmentId { get; init; }
    public Guid? ManagerId { get; init; }
    public DateTime CreatedAt { get; init; }
}
