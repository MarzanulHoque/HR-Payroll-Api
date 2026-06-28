using HRMS.Application.Common.Interfaces;
using HRMS.Application.CompanyConfiguration.Commands.UpdateOrganizationPolicy;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.CompanyConfiguration.Commands;

public class UpdateOrganizationPolicyCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatesPolicy_WhenNoPolicyExists()
    {
        var contextMock = new Mock<IApplicationDbContext>();
        var policies = new List<OrganizationPolicy>();

        contextMock.Setup(x => x.OrganizationPolicies).ReturnsDbSet(policies);
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new UpdateOrganizationPolicyCommandHandler(contextMock.Object);
        var command = new UpdateOrganizationPolicyCommand(
            "HRMS",
            "Monday-Friday",
            "09:00-18:00",
            "UTC",
            "USD",
            "Standard",
            "Standard",
            "Standard",
            true);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal("HRMS", result.Data!.OrganizationName);
        Assert.Equal("Organization policy updated successfully.", result.Message);
        contextMock.Verify(x => x.OrganizationPolicies.Add(It.IsAny<OrganizationPolicy>()), Times.Once);
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}