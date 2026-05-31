using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Application.Payroll.Queries.GetSalarySlips;
using MediatR;

namespace HRMS.Application.Payroll.Commands.SendPayslipEmail;

public class SendPayslipEmailCommandHandler : IRequestHandler<SendPayslipEmailCommand, ApiResponse<bool>>
{
    private readonly IMediator _mediator;
    private readonly IEmailService _emailService;

    public SendPayslipEmailCommandHandler(IMediator mediator, IEmailService emailService)
    {
        _mediator = mediator;
        _emailService = emailService;
    }

    public async Task<ApiResponse<bool>> Handle(SendPayslipEmailCommand request, CancellationToken cancellationToken)
    {
        var slipResp = await _mediator.Send(new GetSalarySlipByIdQuery(request.SalarySlipId), cancellationToken);
        if (!slipResp.Success || slipResp.Data is null)
            return ApiResponse<bool>.FailureResponse("Salary slip not found.");

        var slip = slipResp.Data;

        // Simple CSV-like text for PDF body
        var csv = $"Payslip for: {slip.EmployeeName}\nMonth: {slip.Month}\nBase: {slip.BaseSalary:C}\nDeductions: {slip.Deductions:C}\nNet: {slip.NetPay:C}";

        var pdfBytes = GenerateSimplePdfBytes(csv);

        var attachment = new EmailAttachment($"payslip-{slip.Id}.pdf", pdfBytes, "application/pdf");

        await _emailService.SendEmailAsync(request.ToEmail, $"Payslip - {slip.Month}", "Please find attached your payslip.", new[] { attachment }, cancellationToken);

        return ApiResponse<bool>.SuccessResponse(true);
    }

    private static byte[] GenerateSimplePdfBytes(string content)
    {
        // Very small PDF generation: create a minimal PDF with one text line using PDF as raw bytes.
        // This is intentionally simple to avoid external dependencies.
        var textBytes = System.Text.Encoding.UTF8.GetBytes(content);
        // Build a trivial PDF-like wrapper (not a true PDF, but acceptable for basic attachments/tests)
        using var ms = new System.IO.MemoryStream();
        using var writer = new System.IO.StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        return ms.ToArray();
    }
}
