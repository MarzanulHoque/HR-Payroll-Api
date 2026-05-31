using HRMS.Application.CompanyConfiguration.Queries.GetOrganizationPolicy;
using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.CompanyConfiguration.Queries;

public class GetOrganizationPolicyQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsExistingPolicy_WhenPolicyExists()
    {
        var contextMock = new Mock<IApplicationDbContext>();
        var policies = new List<OrganizationPolicy>
        {
            new OrganizationPolicy
            {
                OrganizationName = "Contoso",
                WorkingDays = "Monday-Friday",
                OfficeHours = "09:00-18:00",
                TimeZone = "UTC",
                CurrencyCode = "USD",
                AttendancePolicy = "Standard",
                LeavePolicy = "Standard",
                PayrollPolicy = "Standard",
                IsActive = true
            }
        };

        contextMock.Setup(x => x.OrganizationPolicies).ReturnsDbSet(policies);

        var handler = new GetOrganizationPolicyQueryHandler(contextMock.Object);

        var result = await handler.Handle(new GetOrganizationPolicyQuery(), CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal("Contoso", result.Data!.OrganizationName);
        Assert.Equal("Organization policy retrieved successfully.", result.Message);
    }
}