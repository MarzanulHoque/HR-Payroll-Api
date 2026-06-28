using Microsoft.AspNetCore.Authorization;

namespace HRMS.API.Authorization;

public sealed class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    public PermissionAuthorizeAttribute(string permission)
    {
        Policy = $"permission:{permission}";
    }
}
