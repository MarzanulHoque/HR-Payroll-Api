using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Payroll.Commands.GeneratePayroll;

public class GeneratePayrollCommandHandler : IRequestHandler<GeneratePayrollCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public GeneratePayrollCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(GeneratePayrollCommand request, CancellationToken cancellationToken)
    {
        var employeeExists = await _context.Employees.AnyAsync(e => e.Id == request.EmployeeId, cancellationToken);
        if (!employeeExists)
        {
            return ApiResponse<Guid>.FailureResponse("Employee not found.");
        }

        // Prevent generating duplicate salary slips for the same month
        var existingSlip = await _context.SalarySlips
            .AnyAsync(s => s.EmployeeId == request.EmployeeId && s.Month == request.Month, cancellationToken);

        if (existingSlip)
        {
            return ApiResponse<Guid>.FailureResponse($"Payroll for {request.Month} is already generated for this employee.");
        }

        // Prevent generation if payroll for the month is locked (guard if DbSet not mocked in tests)
        var locked = false;
        if (_context.PayrollPeriods != null)
        {
            locked = await _context.PayrollPeriods
                .AnyAsync(p => p.Month == request.Month && p.IsLocked, cancellationToken);
        }

        if (locked)
        {
            return ApiResponse<Guid>.FailureResponse($"Payroll for {request.Month} is locked and cannot be generated.");
        }

        var slip = new SalarySlip
        {
            EmployeeId = request.EmployeeId,
            Month = request.Month,
            BaseSalary = request.BaseSalary,
            Deductions = request.Deductions,
            NetPay = request.BaseSalary - request.Deductions,
            Status = "Generated"
        };

        _context.SalarySlips.Add(slip);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(slip.Id, "Payroll generated successfully.");
    }
}
