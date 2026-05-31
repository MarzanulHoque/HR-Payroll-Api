using HRMS.Application.Auth.Commands.Logout;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Auth;

public class LogoutCommandHandlerTests
{
    [Fact]
    public async Task Handle_RevokesActiveRefreshTokens_WhenTokensExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

            var tokens = new List<RefreshToken>
        {
            new RefreshToken { TokenHash = Hash("t1"), UserId = userId, Expires = DateTime.UtcNow.AddDays(7) },
            new RefreshToken { TokenHash = Hash("t2"), UserId = userId, Expires = DateTime.UtcNow.AddDays(7) }
        };

        var contextMock = new Mock<IApplicationDbContext>();
        contextMock.Setup(x => x.RefreshTokens).ReturnsDbSet(tokens);
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new LogoutCommandHandler(contextMock.Object);

        static string Hash(string t)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(t);
            return Convert.ToHexString(sha.ComputeHash(bytes));
        }

        // Act
        var result = await handler.Handle(new LogoutCommand(userId), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.All(tokens, t => Assert.NotNull(t.Revoked));
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenNoActiveTokens()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var contextMock = new Mock<IApplicationDbContext>();
        contextMock.Setup(x => x.RefreshTokens).ReturnsDbSet(new List<RefreshToken>());

        var handler = new LogoutCommandHandler(contextMock.Object);

        // Act
        var result = await handler.Handle(new LogoutCommand(userId), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.True(result.Data);
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
