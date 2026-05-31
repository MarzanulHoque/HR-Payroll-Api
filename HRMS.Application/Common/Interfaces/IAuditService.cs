using System;
using System.Threading.Tasks;

namespace HRMS.Application.Common.Interfaces;

public interface IAuditService
{
    Task RecordAsync(string action, string entityName, Guid? entityId, string performedBy, string? oldValue = null, string? newValue = null, string? ipAddress = null);
}
