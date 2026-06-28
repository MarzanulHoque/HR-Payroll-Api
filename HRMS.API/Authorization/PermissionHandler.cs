using System.Security.Claims;
using HRMS.Application.Common.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.API.Authorization;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionService _permissionService;

    public PermissionHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User == null)
        {
            context.Fail();
            return;
        }

        var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(idClaim, out var userId))
        {
            context.Fail();
            return;
        }

        var has = await _permissionService.UserHasPermissionAsync(userId, requirement.PermissionName);
        if (has) context.Succeed(requirement);
        else context.Fail();
    }
}
