namespace HRMS.Domain.Entities;

public class OrganizationPolicy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string OrganizationName { get; set; } = "HRMS";
    public string WorkingDays { get; set; } = "Monday-Friday";
    public string OfficeHours { get; set; } = "09:00-18:00";
    public string TimeZone { get; set; } = "UTC";
    public string CurrencyCode { get; set; } = "USD";
    public string AttendancePolicy { get; set; } = "Standard";
    public string LeavePolicy { get; set; } = "Standard";
    public string PayrollPolicy { get; set; } = "Standard";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}