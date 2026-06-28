using System;

namespace HRMS.Domain.Entities;

public class PayrollAdjustment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SalarySlipId { get; set; }
    public SalarySlip? SalarySlip { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
