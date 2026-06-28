namespace HRMS.Application.Common.Models;

public class OrganizationPolicyDto
{
    public Guid Id { get; set; }
    public string OrganizationName { get; set; } = string.Empty;
    public string WorkingDays { get; set; } = string.Empty;
    public string OfficeHours { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public string AttendancePolicy { get; set; } = string.Empty;
    public string LeavePolicy { get; set; } = string.Empty;
    public string PayrollPolicy { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}