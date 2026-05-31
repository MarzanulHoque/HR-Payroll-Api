using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HRMS.API.Controllers.Admin;
using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Auth.RBAC;

public class RoleAssignmentTests
{
    [Fact]
    public async Task AssignRoleToUser_AddsUserRole_WhenNotExists()
    {
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        var users = new List<User> { new User { Id = userId, Email = "u@x.local" } };
        var roles = new List<Role> { new Role { Id = roleId, Name = "Employee" } };
        var userRoles = new List<UserRole>();

        var contextMock = new Mock<IApplicationDbContext>();
        contextMock.Setup(c => c.Users).ReturnsDbSet(users);
        contextMock.Setup(c => c.Roles).ReturnsDbSet(roles);
        contextMock.Setup(c => c.UserRoles).ReturnsDbSet(userRoles);
        contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var controller = new RolesPermissionsController(contextMock.Object);

        var result = await controller.AssignRoleToUser(roleId, userId);

        Assert.IsType<NoContentResult>(result);
        Assert.Contains(userRoles, ur => ur.RoleId == roleId && ur.UserId == userId);
    }

    [Fact]
    public async Task RemoveRoleFromUser_RemovesEntry_WhenExists()
    {
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        var existing = new UserRole { UserId = userId, RoleId = roleId };
        var userRoles = new List<UserRole> { existing };

        var contextMock = new Mock<IApplicationDbContext>();
        contextMock.Setup(c => c.UserRoles).ReturnsDbSet(userRoles);
        contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var controller = new RolesPermissionsController(contextMock.Object);

        var result = await controller.RemoveRoleFromUser(roleId, userId);

        Assert.IsType<NoContentResult>(result);
        Assert.DoesNotContain(userRoles, ur => ur.RoleId == roleId && ur.UserId == userId);
    }
}
