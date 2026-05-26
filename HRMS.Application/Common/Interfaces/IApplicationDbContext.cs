using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Common.Interfaces;

/// <summary>
/// Abstraction for the DbContext to enforce Clean Architecture dependency inversion.
/// The Application layer depends on this interface, while the Infrastructure layer implements it.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<Employee> Employees { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
