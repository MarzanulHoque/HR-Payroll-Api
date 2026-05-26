using HRMS.Application.Common.Interfaces;
using HRMS.Application.Employees.Commands.DeleteEmployee;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Employees.Commands;

public class DeleteEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_SoftDeleteEmployee_WhenEmployeeExists()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employee = new Employee { Id = employeeId, IsActive = true };
        
        var contextMock = new Mock<IApplicationDbContext>();
        contextMock.Setup(x => x.Employees).ReturnsDbSet(new List<Employee> { employee });
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new DeleteEmployeeCommandHandler(contextMock.Object);

        // Act
        var result = await handler.Handle(new DeleteEmployeeCommand(employeeId), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.False(employee.IsActive); // Verifies the soft delete
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
