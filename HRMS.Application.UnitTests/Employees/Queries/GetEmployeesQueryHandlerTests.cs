using HRMS.Application.Common.Interfaces;
using HRMS.Application.Employees.Queries.GetEmployees;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Employees.Queries;

public class GetEmployeesQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnOnlyActiveEmployees()
    {
        // Arrange
        var contextMock = new Mock<IApplicationDbContext>();

        var employees = new List<Employee>
        {
            new Employee { Id = Guid.NewGuid(), FirstName = "Active1", IsActive = true },
            new Employee { Id = Guid.NewGuid(), FirstName = "Active2", IsActive = true },
            new Employee { Id = Guid.NewGuid(), FirstName = "Inactive1", IsActive = false }
        };

        contextMock.Setup(x => x.Employees).ReturnsDbSet(employees);

        var handler = new GetEmployeesQueryHandler(contextMock.Object);

        // Act
        var result = await handler.Handle(new GetEmployeesQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count);
        Assert.DoesNotContain(result.Data, e => !e.IsActive);
    }
}
