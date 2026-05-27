using HRMS.Application.Common.Interfaces;
using HRMS.Application.LeaveRequests.Commands.ProcessLeaveRequest;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.LeaveRequests.Commands;

public class ProcessLeaveRequestCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_When_ValidCommand()
    {
        // Arrange
        var contextMock = new Mock<IApplicationDbContext>();
        
        var leaveRequestId = Guid.NewGuid();
        var leaveRequestList = new List<LeaveRequest> 
        { 
            new LeaveRequest 
            { 
                Id = leaveRequestId, 
                EmployeeId = Guid.NewGuid(), 
                Status = "Pending" 
            } 
        };
        
        // Mock FindAsync using Setup implementation since ReturnsDbSet doesn't mock FindAsync implicitly for all scenarios nicely
        contextMock.Setup(x => x.LeaveRequests.FindAsync(new object[] { leaveRequestId }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(leaveRequestList[0]);
            
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new ProcessLeaveRequestCommandHandler(contextMock.Object);

        var command = new ProcessLeaveRequestCommand(leaveRequestId, "Approved");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Leave request approved successfully.", result.Message);
        Assert.Equal("Approved", leaveRequestList[0].Status);
        
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
