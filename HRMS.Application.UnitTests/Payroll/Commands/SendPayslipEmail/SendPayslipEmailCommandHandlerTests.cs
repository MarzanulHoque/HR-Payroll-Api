using HRMS.Application.Common.Interfaces;
using HRMS.Application.Payroll.Commands.SendPayslipEmail;
using HRMS.Application.Payroll.Queries.GetSalarySlips;
using HRMS.Application.Common.Models;
using MediatR;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using Xunit;

namespace HRMS.Application.UnitTests.Payroll.Commands.SendPayslipEmail;

public class SendPayslipEmailCommandHandlerTests
{
    [Fact]
    public async Task Handle_SendsEmailWithPdfAttachment_WhenSalarySlipExists()
    {
        var slipId = Guid.NewGuid();
        var slip = new SalarySlipDto
        {
            Id = slipId,
            EmployeeId = Guid.NewGuid(),
            EmployeeName = "John Doe",
            EmployeeEmail = "john.doe@example.com",
            Month = "May 2026",
            BaseSalary = 1000m,
            Deductions = 100m,
            NetPay = 900m,
            Status = "Paid",
            CreatedAt = DateTime.UtcNow
        };

        var emailService = new FakeEmailService();
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.Is<GetSalarySlipByIdQuery>(q => q.Id == slipId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<SalarySlipDto>.SuccessResponse(slip));
        mediatorMock.Setup(m => m.Send(It.Is<GetSalarySlipByIdQuery>(q => q.Id != slipId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<SalarySlipDto>.FailureResponse("not found"));

        var handler = new SendPayslipEmailCommandHandler(mediatorMock.Object, emailService);

        var result = await handler.Handle(new SendPayslipEmailCommand(slipId, "recipient@example.com"), CancellationToken.None);

        Assert.True(result.Success);
        Assert.True(emailService.WasCalled);
        Assert.Contains(slipId.ToString(), emailService.AttachmentFileName);
        Assert.True(emailService.AttachmentContentLength > 0);
    }

    private class FakeEmailService : IEmailService
    {
        public bool WasCalled { get; private set; }
        public string AttachmentFileName { get; private set; } = string.Empty;
        public int AttachmentContentLength { get; private set; }

        public Task SendEmailAsync(string to, string subject, string body, IEnumerable<HRMS.Application.Common.Models.EmailAttachment>? attachments = null, CancellationToken cancellationToken = default)
        {
            WasCalled = true;
            var first = attachments?.FirstOrDefault();
            if (first != null)
            {
                AttachmentFileName = first.FileName;
                AttachmentContentLength = first.Content?.Length ?? 0;
            }
            return Task.CompletedTask;
        }
    }

    // Using Moq to simulate IMediator in tests above.
}
