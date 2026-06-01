using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Payroll.Commands.AddPayrollAdjustment;

public class AddPayrollAdjustmentCommandHandler : IRequestHandler<AddPayrollAdjustmentCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public AddPayrollAdjustmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(AddPayrollAdjustmentCommand request, CancellationToken cancellationToken)
    {
        var slip = await _context.SalarySlips.FirstOrDefaultAsync(s => s.Id == request.SalarySlipId, cancellationToken);
        if (slip == null)
            return ApiResponse<Guid>.FailureResponse("Salary slip not found.");

        var adj = new PayrollAdjustment
        {
            SalarySlipId = request.SalarySlipId,
            Amount = request.Amount,
            Reason = request.Reason
        };

        _context.PayrollAdjustments.Add(adj);

        // Apply adjustment to the slip for auditability
        slip.NetPay += request.Amount;
        slip.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return ApiResponse<Guid>.SuccessResponse(adj.Id, "Payroll adjustment recorded.");
    }
}
