namespace HRMS.Application.Employees.Queries.GetEmployees;

public record EmployeeDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Department { get; init; }
    public string? Designation { get; init; }
    public bool IsActive { get; init; }
}
