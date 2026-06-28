namespace HRMS.Application.Payroll.Queries.GetSalarySlips;

public record SalarySlipDto
{
    public Guid Id { get; init; }
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; } = string.Empty;
    public string EmployeeEmail { get; init; } = string.Empty;
    public string Month { get; init; } = string.Empty;
    public decimal BaseSalary { get; init; }
    public decimal Deductions { get; init; }
    public decimal NetPay { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
