namespace HRMS.Application.Common.Interfaces.Auth;

public interface IPermissionService
{
    Task<bool> UserHasPermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default);
}
