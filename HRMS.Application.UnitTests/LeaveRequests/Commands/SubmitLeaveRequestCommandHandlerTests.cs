using HRMS.Application.Common.Interfaces;
using HRMS.Application.LeaveRequests.Commands.SubmitLeaveRequest;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.LeaveRequests.Commands;

public class SubmitLeaveRequestCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_When_ValidCommand()
    {
        // Arrange
        var contextMock = new Mock<IApplicationDbContext>();
        
        var employeeId = Guid.NewGuid();
        var employeeList = new List<Employee> { new Employee { Id = employeeId, FirstName = "Test", LastName = "User", Email = "t@t.com", Department = "HR", Designation = "Manager" } };
        
        contextMock.Setup(x => x.Employees).ReturnsDbSet(employeeList);
        contextMock.Setup(x => x.LeaveRequests).ReturnsDbSet(new List<LeaveRequest>());
        
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new SubmitLeaveRequestCommandHandler(contextMock.Object);

        var command = new SubmitLeaveRequestCommand(
            employeeId,
            "Sick",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(3),
            "Feeling unwell"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Leave request submitted successfully.", result.Message);
        
        contextMock.Verify(x => x.LeaveRequests.Add(It.IsAny<LeaveRequest>()), Times.Once);
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
