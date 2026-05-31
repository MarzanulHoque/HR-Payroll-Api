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

    [Fact]
    public async Task PayrollReport_ReturnsCsv_ForAuthorizedUser()
    {
        var roleId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        await using var factory = new CustomWebApplicationFactory(UserId, roleId, permissionId);
        var client = factory.CreateClient();

        var resp = await client.GetAsync("/api/v1/payroll/report?month=May%202026");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        Assert.True(resp.Content.Headers.ContentType != null);
        Assert.Equal("text/csv", resp.Content.Headers.ContentType.MediaType);

        var body = await resp.Content.ReadAsStringAsync();
        Assert.Contains("EmployeeId,EmployeeName,Month,BaseSalary", body);
    }
}
