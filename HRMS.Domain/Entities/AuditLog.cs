using System;

namespace HRMS.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EntityName { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }
    public string Action { get; set; } = string.Empty; // Create/Update/Delete/LoginSuccess/LoginFailed etc.
    public string PerformedBy { get; set; } = string.Empty; // user email or system
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? IpAddress { get; set; }
}
