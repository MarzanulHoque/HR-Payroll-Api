using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Admin;

[ApiController]
[Route("api/admin/roles-permissions")]
[Authorize(Roles = "Admin")]
public class RolesPermissionsController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public RolesPermissionsController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("permissions")]
    public IActionResult GetPermissions()
    {
        var perms = _context.Permissions.Select(p => new { p.Id, p.Name }).ToList();
        return Ok(perms);
    }

    [HttpGet("roles")]
    public IActionResult GetRoles()
    {
        var roles = _context.Roles.Select(r => new { r.Id, r.Name }).ToList();
        return Ok(roles);
    }

    [HttpPost("permissions")]
    public async Task<IActionResult> CreatePermission([FromBody] Permission dto)
    {
        var p = new Permission { Name = dto.Name };
        _context.Permissions.Add(p);
        await _context.SaveChangesAsync(CancellationToken.None);
        return CreatedAtAction(nameof(GetPermissions), new { id = p.Id }, p);
    }

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole([FromBody] Role dto)
    {
        var r = new Role { Name = dto.Name };
        _context.Roles.Add(r);
        await _context.SaveChangesAsync(CancellationToken.None);
        return CreatedAtAction(nameof(GetPermissions), new { id = r.Id }, r);
    }

    [HttpPost("roles/{roleId}/permissions/{permissionId}")]
    public async Task<IActionResult> AssignPermission(Guid roleId, Guid permissionId)
    {
        var exists = _context.RolePermissions.Any(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        if (exists) return NoContent();

        _context.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permissionId });
        await _context.SaveChangesAsync(CancellationToken.None);
        return NoContent();
    }

    [HttpPost("roles/{roleId}/users/{userId}")]
    public async Task<IActionResult> AssignRoleToUser(Guid roleId, Guid userId)
    {
        var exists = _context.UserRoles.Any(ur => ur.RoleId == roleId && ur.UserId == userId);
        if (exists) return NoContent();

        // Ensure user and role exist
        var userExists = _context.Users.Any(u => u.Id == userId);
        var roleExists = _context.Roles.Any(r => r.Id == roleId);
        if (!userExists || !roleExists) return NotFound();

        _context.UserRoles.Add(new UserRole { RoleId = roleId, UserId = userId });
        await _context.SaveChangesAsync(CancellationToken.None);
        return NoContent();
    }

    [HttpGet("users/{userId}/roles")]
    public IActionResult GetUserRoles(Guid userId)
    {
        var roles = _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => new { ur.RoleId, RoleName = ur.Role.Name })
            .ToList();

        if (!roles.Any()) return NotFound();
        return Ok(roles);
    }

    [HttpDelete("roles/{roleId}/users/{userId}")]
    public async Task<IActionResult> RemoveRoleFromUser(Guid roleId, Guid userId)
    {
        var existing = _context.UserRoles.SingleOrDefault(ur => ur.RoleId == roleId && ur.UserId == userId);
        if (existing == null) return NotFound();

        _context.UserRoles.Remove(existing);
        await _context.SaveChangesAsync(CancellationToken.None);
        return NoContent();
    }

    [HttpGet("roles/{roleId}/permissions")]
    public IActionResult GetRolePermissions(Guid roleId)
    {
        var perms = _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => new { rp.PermissionId, PermissionName = rp.Permission.Name })
            .ToList();

        if (!perms.Any()) return NotFound();
        return Ok(perms);
    }

    [HttpDelete("roles/{roleId}/permissions/{permissionId}")]
    public async Task<IActionResult> RemovePermissionFromRole(Guid roleId, Guid permissionId)
    {
        var existing = _context.RolePermissions.SingleOrDefault(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        if (existing == null) return NotFound();

        _context.RolePermissions.Remove(existing);
        await _context.SaveChangesAsync(CancellationToken.None);
        return NoContent();
    }
}
