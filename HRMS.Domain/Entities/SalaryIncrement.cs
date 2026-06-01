using System;

namespace HRMS.Domain.Entities;

public class SalaryIncrement
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public decimal Amount { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
