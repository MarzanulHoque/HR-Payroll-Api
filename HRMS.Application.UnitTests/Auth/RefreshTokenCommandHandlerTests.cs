using HRMS.Application.Auth.Commands.RefreshToken;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Interfaces.Auth;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Auth;

public class RefreshTokenCommandHandlerTests
{
    [Fact]
    public async Task Handle_RefreshesToken_WhenExistingTokenIsActive()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@hrms.local",
            FirstName = "Test",
            PasswordHash = "hash",
            IsActive = true
        };

        var role = new Role { Id = Guid.NewGuid(), Name = "User" };
        user.UserRoles.Add(new UserRole { Role = role, RoleId = role.Id, User = user, UserId = user.Id });

        var existingToken = new RefreshToken
        {
            Token = "old-refresh-token",
            Expires = DateTime.UtcNow.AddDays(1),
            User = user,
            UserId = user.Id
        };

        var contextMock = new Mock<IApplicationDbContext>();
        contextMock.Setup(x => x.RefreshTokens).ReturnsDbSet(new List<RefreshToken> { existingToken });
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var tokenServiceMock = new Mock<ITokenService>();
        tokenServiceMock.Setup(ts => ts.GenerateAccessToken(It.IsAny<User>(), It.IsAny<IList<string>>()))
            .Returns("new-access-token");
        tokenServiceMock.Setup(ts => ts.GenerateRefreshToken()).Returns("new-refresh-token");

        var handler = new RefreshTokenCommandHandler(contextMock.Object, tokenServiceMock.Object);

        // Act
        var result = await handler.Handle(new RefreshTokenCommand("old-refresh-token"), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("new-access-token", result.Data!.AccessToken);
        Assert.Equal("new-refresh-token", result.Data.RefreshToken);
        Assert.NotNull(existingToken.Revoked);
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
