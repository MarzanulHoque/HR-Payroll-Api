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
}
