using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Payroll.Commands.LockPayroll;

public class LockPayrollCommandHandler : IRequestHandler<LockPayrollCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public LockPayrollCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(LockPayrollCommand request, CancellationToken cancellationToken)
    {
        var existing = await _context.PayrollPeriods.FirstOrDefaultAsync(p => p.Month == request.Month, cancellationToken);
        if (existing != null)
        {
            if (existing.IsLocked)
                return ApiResponse<bool>.FailureResponse("Payroll already locked for this month.");

            existing.IsLocked = true;
            existing.LockedAt = DateTime.UtcNow;
        }
        else
        {
            _context.PayrollPeriods.Add(new PayrollPeriod { Month = request.Month, IsLocked = true, LockedAt = DateTime.UtcNow });
        }

        await _context.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResponse(true, "Payroll locked for month.");
    }
}
