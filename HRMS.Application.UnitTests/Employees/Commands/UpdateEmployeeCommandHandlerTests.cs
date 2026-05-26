using HRMS.Application.Common.Interfaces;
using HRMS.Application.Employees.Commands.UpdateEmployee;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Employees.Commands;

public class UpdateEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_UpdateEmployeeAndReturnSuccess_WhenEmployeeExists()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employee = new Employee 
        { 
            Id = employeeId, 
            FirstName = "OldName", 
            LastName = "OldLast",
            Email = "old@test.com"
        };
        
        var contextMock = new Mock<IApplicationDbContext>();
        contextMock.Setup(x => x.Employees).ReturnsDbSet(new List<Employee> { employee });
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new UpdateEmployeeCommandHandler(contextMock.Object);
        var command = new UpdateEmployeeCommand(employeeId, "NewName", "NewLast", "new@test.com", "IT", "Dev");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(employeeId, result.Data);
        Assert.Equal("NewName", employee.FirstName);
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
