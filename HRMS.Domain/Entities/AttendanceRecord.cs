using System;

namespace HRMS.Domain.Entities;

public class AttendanceRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Links to Employee
    public Guid EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    
    public DateTime Date { get; set; } // The calendar date for this record
    public DateTime ClockInTime { get; set; }
    public DateTime? ClockOutTime { get; set; }
    
    // Notes or status if needed (e.g. Late, OnTime)
    public string? Status { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
