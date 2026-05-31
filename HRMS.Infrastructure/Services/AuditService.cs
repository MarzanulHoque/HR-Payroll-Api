using System;
using System.Threading.Tasks;
using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Entities;

namespace HRMS.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly Data.ApplicationDbContext _db;

    public AuditService(Data.ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task RecordAsync(string action, string entityName, Guid? entityId, string performedBy, string? oldValue = null, string? newValue = null, string? ipAddress = null)
    {
        var log = new AuditLog
        {
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            PerformedBy = performedBy,
            OldValue = oldValue,
            NewValue = newValue,
            IpAddress = ipAddress,
            Timestamp = DateTime.UtcNow
        };

        _db.AuditLogs.Add(log);
        await _db.SaveChangesAsync(default);
    }
}
