using HRMS.Application.Attendance.Commands.ClockIn;
using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Attendance.Commands;

public class ClockInCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_When_ValidCommand()
    {
        // Arrange
        var contextMock = new Mock<IApplicationDbContext>();
        
        var employeeId = Guid.NewGuid();
        var employeeList = new List<Employee> { new Employee { Id = employeeId, FirstName = "John", LastName = "Doe", Email = "j@d.com", Department = "IT", Designation = "Dev" } };
        
        contextMock.Setup(x => x.Employees).ReturnsDbSet(employeeList);
        contextMock.Setup(x => x.AttendanceRecords).ReturnsDbSet(new List<AttendanceRecord>());
        
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new ClockInCommandHandler(contextMock.Object);

        var command = new ClockInCommand(employeeId, "On Time");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Clocked in successfully.", result.Message);
        
        contextMock.Verify(x => x.AttendanceRecords.Add(It.IsAny<AttendanceRecord>()), Times.Once);
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
