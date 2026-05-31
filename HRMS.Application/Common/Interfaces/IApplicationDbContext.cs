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
    DbSet<Department> Departments { get; }
    DbSet<Designation> Designations { get; }
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<AttendanceRecord> AttendanceRecords { get; }
    DbSet<LeaveRequest> LeaveRequests { get; }
    DbSet<SalarySlip> SalarySlips { get; }
    DbSet<OrganizationPolicy> OrganizationPolicies { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
