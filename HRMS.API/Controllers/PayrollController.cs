using HRMS.Application.Common.Models;
using HRMS.Application.Payroll.Commands.GeneratePayroll;
using HRMS.Application.Payroll.Queries.GetSalarySlips;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using HRMS.Application.Payroll.Commands.LockPayroll;
using HRMS.Application.Payroll.Commands.AddSalaryIncrement;
using HRMS.Application.Payroll.Commands.AddPayrollAdjustment;
using System;
using HRMS.API.Hubs;

namespace HRMS.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class PayrollController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHubContext<NotificationsHub>? _hubContext;

    public PayrollController(IMediator mediator, IHubContext<NotificationsHub>? hubContext = null)
    {
        _mediator = mediator;
        _hubContext = hubContext;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<ApiResponse<Guid>>> GeneratePayroll(GeneratePayrollCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.Success)
        {
            await BroadcastDashboardUpdate("payroll.generate", result.Data);
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("slips")]
    public async Task<ActionResult<ApiResponse<List<SalarySlipDto>>>> GetAllSalarySlips([FromQuery] string? month, [FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] bool desc = false, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
    {
        var parameters = new SalarySlipsQueryParameters(month, search, sortBy, desc, page, pageSize);
        var result = await _mediator.Send(new GetAllSalarySlipsQuery(parameters));
        return Ok(result);
    }

    [HttpGet("report")]
    [Authorize(Policy = "payroll.generate")]
    public async Task<IActionResult> GetPayrollReport([FromQuery] string month)
    {
        if (string.IsNullOrWhiteSpace(month))
            return BadRequest(ApiResponse<string>.FailureResponse("Month query parameter is required (e.g. 'May 2026')."));

        var parameters = new SalarySlipsQueryParameters(month, null, null, false, 1, 1000);
        var result = await _mediator.Send(new GetAllSalarySlipsQuery(parameters));
        if (!result.Success)
            return BadRequest(result);

        var slips = result.Data;

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("EmployeeId,EmployeeName,Month,BaseSalary,Deductions,NetPay,Status,CreatedAt");
        foreach (var s in slips)
        {
            sb.AppendLine($"{s.EmployeeId},{EscapeCsv(s.EmployeeName)},{s.Month},{s.BaseSalary},{s.Deductions},{s.NetPay},{s.Status},{s.CreatedAt:O}");
        }

        var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        var fileName = $"payroll_{month.Replace(' ', '_')}.csv";
        return File(bytes, "text/csv", fileName);
    }

    [HttpGet("slips/{id}/download")]
    [Authorize(Policy = "payroll.generate")]
    public async Task<IActionResult> DownloadSalarySlip([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetSalarySlipByIdQuery(id));
        if (!result.Success)
            return NotFound(result);

        var s = result.Data;
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("EmployeeId,EmployeeName,Month,BaseSalary,Deductions,NetPay,Status,CreatedAt");
        sb.AppendLine($"{s.EmployeeId},{EscapeCsv(s.EmployeeName)},{s.Month},{s.BaseSalary},{s.Deductions},{s.NetPay},{s.Status},{s.CreatedAt:O}");

        var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        var fileName = $"payslip_{s.Id}.csv";
        return File(bytes, "text/csv", fileName);
    }

    [HttpGet("slips/{id}/pdf")]
    [Authorize(Policy = "payroll.generate")]
    public async Task<IActionResult> DownloadSalarySlipPdf([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetSalarySlipByIdQuery(id));
        if (!result.Success)
            return NotFound(result);

        var s = result.Data;
        var text = $"Employee: {s.EmployeeName}\nMonth: {s.Month}\nBaseSalary: {s.BaseSalary}\nDeductions: {s.Deductions}\nNetPay: {s.NetPay}\nStatus: {s.Status}\nGeneratedAt: {s.CreatedAt:O}";
        var bytes = GenerateSimplePdf(text);
        var fileName = $"payslip_{s.Id}.pdf";
        return File(bytes, "application/pdf", fileName);
    }

    [HttpPost("slips/{id}/email")]
    [Authorize(Policy = "payroll.generate")]
    public async Task<IActionResult> EmailSalarySlip([FromRoute] Guid id, [FromBody] EmailRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.To))
            return BadRequest(ApiResponse<string>.FailureResponse("Recipient email is required."));

        var result = await _mediator.Send(new HRMS.Application.Payroll.Commands.SendPayslipEmail.SendPayslipEmailCommand(id, request.To));
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("lock")]
    [Authorize(Policy = "payroll.generate")]
    public async Task<IActionResult> LockPayroll([FromBody] LockPayrollRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Month))
            return BadRequest(ApiResponse<string>.FailureResponse("Month is required."));

        var resp = await _mediator.Send(new LockPayrollCommand(request.Month));
        if (!resp.Success) return BadRequest(resp);
        await BroadcastDashboardUpdate("payroll.lock", request.Month);
        return Ok(resp);
    }

    [HttpPost("increments")]
    [Authorize(Policy = "payroll.manage")]
    public async Task<IActionResult> AddSalaryIncrement([FromBody] AddSalaryIncrementRequest request)
    {
        if (request == null)
            return BadRequest(ApiResponse<string>.FailureResponse("Invalid request."));

        var resp = await _mediator.Send(new AddSalaryIncrementCommand(request.EmployeeId, request.Amount, request.EffectiveFrom, request.Reason));
        if (!resp.Success) return BadRequest(resp);
        await BroadcastDashboardUpdate("payroll.increment", resp.Data);
        return Ok(resp);
    }

    [HttpPost("adjustments")]
    [Authorize(Policy = "payroll.manage")]
    public async Task<IActionResult> AddPayrollAdjustment([FromBody] AddPayrollAdjustmentRequest request)
    {
        if (request == null)
            return BadRequest(ApiResponse<string>.FailureResponse("Invalid request."));

        var resp = await _mediator.Send(new AddPayrollAdjustmentCommand(request.SalarySlipId, request.Amount, request.Reason));
        if (!resp.Success) return BadRequest(resp);
        await BroadcastDashboardUpdate("payroll.adjustment", resp.Data);
        return Ok(resp);
    }

    private async Task BroadcastDashboardUpdate(string source, object entityId)
    {
        if (_hubContext == null)
        {
            return;
        }

        try
        {
            await _hubContext.Clients.All.SendAsync("DashboardUpdated", new
            {
                Source = source,
                EntityId = entityId,
                Timestamp = DateTime.UtcNow
            });
        }
        catch
        {
            // best-effort only
        }
    }

    private static byte[] GenerateSimplePdf(string text)
    {
        // Build a very small PDF containing the provided text. This is not feature-complete
        // but sufficient for simple payslip export without external dependencies.
        var sb = new System.Text.StringBuilder();
        var objects = new List<string>();

        // Contents stream: draw text lines
        var lines = text.Split('\n');
        var contentSb = new System.Text.StringBuilder();
        contentSb.AppendLine("BT");
        contentSb.AppendLine("/F1 12 Tf");
        contentSb.AppendLine("50 750 Td");
        for (int i = 0; i < lines.Length; i++)
        {
            var escaped = lines[i].Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
            contentSb.AppendLine("(" + escaped + ") Tj");
            if (i < lines.Length - 1)
                contentSb.AppendLine("0 -14 Td");
        }
        contentSb.AppendLine("ET");
        var content = contentSb.ToString();

        // object 1: Catalog
        objects.Add("1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n");
        // object 2: Pages
        objects.Add("2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n");
        // object 3: Page
        objects.Add("3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>\nendobj\n");
        // object 4: Font
        objects.Add("4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n");
        // object 5: Contents stream
        var contentBytes = System.Text.Encoding.ASCII.GetBytes(content);
        var stream = $"5 0 obj\n<< /Length {contentBytes.Length} >>\nstream\n{content}endstream\nendobj\n";
        objects.Add(stream);

        // assemble with xref
        var header = "%PDF-1.1\n%âãÏÓ\n";
        var pdf = new System.Text.StringBuilder();
        pdf.Append(header);

        var offsets = new List<int>();
        foreach (var obj in objects)
        {
            offsets.Add(System.Text.Encoding.ASCII.GetByteCount(pdf.ToString()));
            pdf.Append(obj);
        }

        var xrefStart = System.Text.Encoding.ASCII.GetByteCount(pdf.ToString());
        pdf.AppendLine("xref");
        pdf.AppendLine($"0 {objects.Count + 1}");
        pdf.AppendLine("0000000000 65535 f ");
        for (int i = 0; i < offsets.Count; i++)
        {
            pdf.AppendLine(offsets[i].ToString("D10") + " 00000 n ");
        }

        pdf.AppendLine("trailer");
        pdf.AppendLine($"<< /Size {objects.Count + 1} /Root 1 0 R >>");
        pdf.AppendLine("startxref");
        pdf.AppendLine(xrefStart.ToString());
        pdf.AppendLine("%%EOF");

        return System.Text.Encoding.ASCII.GetBytes(pdf.ToString());
    }

    private static string EscapeCsv(string input)
    {
        if (input.Contains(',') || input.Contains('"') || input.Contains('\n'))
        {
            return '"' + input.Replace("\"", "\"\"") + '"';
        }
        return input;
    }
}

public class EmailRequest
{
    public string To { get; set; } = string.Empty;
}

public class LockPayrollRequest
{
    public string Month { get; set; } = string.Empty;
}

public class AddSalaryIncrementRequest
{
    public Guid EmployeeId { get; set; }
    public decimal Amount { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class AddPayrollAdjustmentRequest
{
    public Guid SalarySlipId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
}
