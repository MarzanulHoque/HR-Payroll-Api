using HRMS.Application.Common.Interfaces;
using HRMS.Application.Employees.Commands.CreateEmployee;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Employees.Commands;

public class CreateEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_When_ValidCommand()
    {
        // Arrange
        var contextMock = new Mock<IApplicationDbContext>();
        
        // Mock DbContext DbSet using Moq.EntityFrameworkCore
        contextMock.Setup(x => x.Employees).ReturnsDbSet(new List<Employee>());
        
        // Mock SaveChangesAsync
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CreateEmployeeCommandHandler(contextMock.Object);

        var command = new CreateEmployeeCommand(
            "John",
            "Doe",
            "john.doe@example.com",
            "IT",
            "Developer"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Employee created successfully.", result.Message);
        Assert.NotEqual(Guid.Empty, result.Data);
        
        // Verify Add and SaveChanges were called exactly once
        contextMock.Verify(x => x.Employees.Add(It.IsAny<Employee>()), Times.Once);
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
