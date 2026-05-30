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
}
