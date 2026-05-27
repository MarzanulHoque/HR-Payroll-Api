using HRMS.Application.Common.Interfaces;
using HRMS.Application.Dashboard.Queries.GetDashboardMetrics;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Dashboard.Queries;

public class GetDashboardMetricsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnCorrectDetailedMetrics()
    {
        // Arrange
        var contextMock = new Mock<IApplicationDbContext>();
        
        var employees = new List<Employee> { new Employee(), new Employee() }; 
        var leaves = new List<LeaveRequest> { new LeaveRequest { Status = "Pending" }, new LeaveRequest { Status = "Approved" } }; 
        var attendance = new List<AttendanceRecord> { new AttendanceRecord { Date = DateTime.UtcNow.Date } };

        contextMock.Setup(x => x.Employees).ReturnsDbSet(employees);
        contextMock.Setup(x => x.LeaveRequests).ReturnsDbSet(leaves);
        contextMock.Setup(x => x.AttendanceRecords).ReturnsDbSet(attendance);

        var handler = new GetDashboardMetricsQueryHandler(contextMock.Object);

        // Act
        var result = await handler.Handle(new GetDashboardMetricsQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, result.Data.TotalEmployees);
        Assert.Equal(1, result.Data.PendingLeaveRequests);
        Assert.Equal(1, result.Data.TotalAttendanceToday);
    }
}
