using System.Collections.Generic;

namespace HRMS.Application.Departments.Queries.GetDepartmentTree;

using HRMS.Application.Common.Dtos;

public record DepartmentTreeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid? ManagerId { get; init; }
    public EmployeeMiniDto? Manager { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<DepartmentTreeDto> Children { get; init; } = new List<DepartmentTreeDto>();
}
