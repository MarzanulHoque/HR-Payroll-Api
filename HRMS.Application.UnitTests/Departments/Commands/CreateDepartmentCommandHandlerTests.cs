using HRMS.Application.Common.Interfaces;
using HRMS.Application.Departments.Commands.CreateDepartment;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Departments.Commands;

public class CreateDepartmentCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_When_ValidCommand()
    {
        // Arrange
        var contextMock = new Mock<IApplicationDbContext>();
        
        contextMock.Setup(x => x.Departments).ReturnsDbSet(new List<Department>());
        
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CreateDepartmentCommandHandler(contextMock.Object);

        var command = new CreateDepartmentCommand(
            "Engineering",
            "Software & Infrastructure",
            null,
            null
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Department created successfully.", result.Message);
        Assert.NotEqual(Guid.Empty, result.Data);
        
        contextMock.Verify(x => x.Departments.Add(It.IsAny<Department>()), Times.Once);
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
