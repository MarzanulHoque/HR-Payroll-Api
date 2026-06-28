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
            TokenHash = Hash("old-refresh-token"),
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

    [Fact]
    public async Task Handle_DetectsReuseAndRevokesAll_WhenTokenWasReplaced()
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

        var oldToken = new RefreshToken
        {
            TokenHash = Hash("old-refresh-token"),
            Expires = DateTime.UtcNow.AddDays(1),
            Revoked = DateTime.UtcNow.AddMinutes(-10),
            ReplacedByTokenHash = Hash("new-refresh-token"),
            User = user,
            UserId = user.Id
        };

        var activeToken = new RefreshToken
        {
            TokenHash = Hash("active-refresh-token"),
            Expires = DateTime.UtcNow.AddDays(1),
            User = user,
            UserId = user.Id
        };

        var tokens = new List<RefreshToken> { oldToken, activeToken };

        var contextMock = new Mock<IApplicationDbContext>();
        contextMock.Setup(x => x.RefreshTokens).ReturnsDbSet(tokens);
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var tokenServiceMock = new Mock<ITokenService>();

        var handler = new RefreshTokenCommandHandler(contextMock.Object, tokenServiceMock.Object);

        // Act
        var result = await handler.Handle(new RefreshTokenCommand("old-refresh-token"), CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("reuse", result.Message, StringComparison.OrdinalIgnoreCase);
        // activeToken should now be revoked
        Assert.NotNull(activeToken.Revoked);
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
    private static string Hash(string t)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(t);
        return Convert.ToHexString(sha.ComputeHash(bytes));
    }
}
