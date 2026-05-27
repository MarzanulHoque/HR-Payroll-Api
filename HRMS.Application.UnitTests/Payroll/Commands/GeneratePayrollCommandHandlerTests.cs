using HRMS.Application.Common.Interfaces;
using HRMS.Application.Payroll.Commands.GeneratePayroll;
using HRMS.Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HRMS.Application.UnitTests.Payroll.Commands;

public class GeneratePayrollCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_When_ValidCommand()
    {
        // Arrange
        var contextMock = new Mock<IApplicationDbContext>();
        
        var employeeId = Guid.NewGuid();
        var employeeList = new List<Employee> { new Employee { Id = employeeId, FirstName = "Test", LastName = "User" } };
        
        contextMock.Setup(x => x.Employees).ReturnsDbSet(employeeList);
        contextMock.Setup(x => x.SalarySlips).ReturnsDbSet(new List<SalarySlip>());
        
        contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new GeneratePayrollCommandHandler(contextMock.Object);

        var command = new GeneratePayrollCommand(
            employeeId,
            "May 2026",
            5000m,
            200m
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Payroll generated successfully.", result.Message);
        
        contextMock.Verify(x => x.SalarySlips.Add(It.IsAny<SalarySlip>()), Times.Once);
        // Verify NetPay logic
        contextMock.Verify(x => x.SalarySlips.Add(It.Is<SalarySlip>(s => s.NetPay == 4800m)), Times.Once);
        contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
