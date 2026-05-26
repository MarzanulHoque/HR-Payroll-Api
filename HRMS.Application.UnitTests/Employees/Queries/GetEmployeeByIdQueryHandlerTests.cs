using HRMS.Application.Common.Interfaces;
using HRMS.Application.Employees.Queries.GetEmployeeById;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Employees.Queries;

public class GetEmployeeByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenEmployeeDoesNotExist()
    {
        // Arrange
        var contextMock = new Mock<IApplicationDbContext>();
        contextMock.Setup(x => x.Employees).ReturnsDbSet(new List<Employee>());

        var handler = new GetEmployeeByIdQueryHandler(contextMock.Object);

        // Act
        var result = await handler.Handle(new GetEmployeeByIdQuery(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Employee not found.", result.Message);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task Handle_Should_ReturnEmployee_WhenEmployeeExistsAndIsActive()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var contextMock = new Mock<IApplicationDbContext>();
        
        var employees = new List<Employee>
        {
            new Employee { Id = employeeId, FirstName = "Test", IsActive = true }
        };

        contextMock.Setup(x => x.Employees).ReturnsDbSet(employees);

        var handler = new GetEmployeeByIdQueryHandler(contextMock.Object);

        // Act
        var result = await handler.Handle(new GetEmployeeByIdQuery(employeeId), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(employeeId, result.Data.Id);
    }
}
