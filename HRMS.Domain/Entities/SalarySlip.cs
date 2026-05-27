using System;

namespace HRMS.Domain.Entities;

public class SalarySlip
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    
    public string Month { get; set; } = string.Empty; // e.g., "May 2026"
    public decimal BaseSalary { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetPay { get; set; }
    
    public string Status { get; set; } = "Generated"; // Generated, Paid
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
