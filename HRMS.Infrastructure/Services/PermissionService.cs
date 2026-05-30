using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Interfaces.Auth;
using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Services;

public class PermissionService : IPermissionService
{
    private readonly IApplicationDbContext _context;

    public PermissionService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserHasPermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default)
    {
        // Check if any of the user's roles has the requested permission
        return await (from ur in _context.UserRoles
                      join rp in _context.RolePermissions on ur.RoleId equals rp.RoleId
                      join p in _context.Permissions on rp.PermissionId equals p.Id
                      where ur.UserId == userId && p.Name == permissionName
                      select p.Id)
            .AnyAsync(cancellationToken);
    }
}
