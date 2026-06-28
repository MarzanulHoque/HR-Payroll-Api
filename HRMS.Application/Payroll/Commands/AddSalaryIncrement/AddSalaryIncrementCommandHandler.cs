using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Payroll.Commands.AddSalaryIncrement;

public class AddSalaryIncrementCommandHandler : IRequestHandler<AddSalaryIncrementCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public AddSalaryIncrementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(AddSalaryIncrementCommand request, CancellationToken cancellationToken)
    {
        var employeeExists = await _context.Employees.AnyAsync(e => e.Id == request.EmployeeId, cancellationToken);
        if (!employeeExists)
            return ApiResponse<Guid>.FailureResponse("Employee not found.");

        var inc = new SalaryIncrement
        {
            EmployeeId = request.EmployeeId,
            Amount = request.Amount,
            EffectiveFrom = request.EffectiveFrom,
            Reason = request.Reason
        };

        _context.SalaryIncrements.Add(inc);
        await _context.SaveChangesAsync(cancellationToken);
        return ApiResponse<Guid>.SuccessResponse(inc.Id, "Salary increment recorded.");
    }
}
