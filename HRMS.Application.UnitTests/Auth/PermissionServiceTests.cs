using HRMS.Application.Common.Interfaces.Auth;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Services;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Auth;

public class PermissionServiceTests
{
    [Fact]
    public async Task UserHasPermissionAsync_ReturnsTrue_WhenPermissionAssignedToRole()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        var userRoles = new List<UserRole> { new UserRole { UserId = userId, RoleId = roleId } };
        var rolePermissions = new List<RolePermission> { new RolePermission { RoleId = roleId, PermissionId = permissionId } };
        var permissions = new List<Permission> { new Permission { Id = permissionId, Name = "payroll.generate" } };

        var mockContext = new Mock<HRMS.Application.Common.Interfaces.IApplicationDbContext>();
        mockContext.Setup(x => x.UserRoles).ReturnsDbSet(userRoles);
        mockContext.Setup(x => x.RolePermissions).ReturnsDbSet(rolePermissions);
        mockContext.Setup(x => x.Permissions).ReturnsDbSet(permissions);

        var service = new PermissionService(mockContext.Object);

        // Act
        var result = await service.UserHasPermissionAsync(userId, "payroll.generate");

        // Assert
        Assert.True(result);
    }
}
