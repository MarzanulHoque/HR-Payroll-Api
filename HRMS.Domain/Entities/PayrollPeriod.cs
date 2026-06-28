using System;

namespace HRMS.Domain.Entities;

public class PayrollPeriod
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Month { get; set; } = string.Empty; // e.g., "May 2026"
    public bool IsLocked { get; set; } = false;
    public DateTime? LockedAt { get; set; }
}
