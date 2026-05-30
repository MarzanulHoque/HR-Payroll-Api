using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HRMS.Application.UnitTests.Integration;

public class IntegrationTests
{
    public static readonly Guid UserId = Guid.NewGuid();

    [Fact]
    public async Task PermissionProtectedEndpoint_AllowsUserWithPermission()
    {
        var roleId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        await using var factory = new CustomWebApplicationFactory(UserId, roleId, permissionId);
        var client = factory.CreateClient();

        var resp = await client.GetAsync("/api/admin/test-protected");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }
}
