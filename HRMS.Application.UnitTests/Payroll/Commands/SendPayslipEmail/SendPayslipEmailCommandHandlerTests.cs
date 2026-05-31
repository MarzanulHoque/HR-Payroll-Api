using HRMS.Application.Common.Interfaces;
using HRMS.Application.Payroll.Commands.SendPayslipEmail;
using HRMS.Application.Payroll.Queries.GetSalarySlips;
using HRMS.Application.Common.Models;
using MediatR;
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
        var mediator = new FakeMediator(request =>
        {
            if (request is GetSalarySlipByIdQuery q && q.Id == slipId)
            {
                return Task.FromResult((object)ApiResponse<SalarySlipDto>.SuccessResponse(slip));
            }
            return Task.FromResult((object)ApiResponse<SalarySlipDto>.FailureResponse("not found"));
        });

        var handler = new SendPayslipEmailCommandHandler(mediator, emailService);

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

    private class FakeMediator : IMediator
    {
        private readonly Func<object, Task<object>> _responder;
        public FakeMediator(Func<object, Task<object>> responder)
        {
            _responder = responder;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var task = _responder(request);
            return task.ContinueWith(t => (TResponse)t.Result, cancellationToken);
        }

        // Not used in these tests
        public Task<object?> Send(object request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task Publish(object notification, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification => throw new NotImplementedException();
    }
}
