using HRMS.Application.Common.Interfaces;
using HRMS.Application.Designations.Commands.CreateDesignation;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Designations.Commands;

public class CreateDesignationCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_When_ValidCommand()
    {
        // Arrange
        var contextMock = new Mock<IApplicationDbContext>();
        
        contextMock.Setup(x => x.Designations).ReturnsDbSet(new List<Designation>());
        
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CreateDesignationCommandHandler(contextMock.Object);

        var command = new CreateDesignationCommand(
            "Senior Backend Developer",
            "Develops server-side logic.",
            Guid.NewGuid(),
            true
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Designation created successfully.", result.Message);
        Assert.NotEqual(Guid.Empty, result.Data);
        
        contextMock.Verify(x => x.Designations.Add(It.IsAny<Designation>()), Times.Once);
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
