using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Payroll.Queries.GetSalarySlips;

public class GetSalarySlipByIdQueryHandler : IRequestHandler<GetSalarySlipByIdQuery, ApiResponse<SalarySlipDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSalarySlipByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<SalarySlipDto>> Handle(GetSalarySlipByIdQuery request, CancellationToken cancellationToken)
    {
        var slip = await _context.SalarySlips
            .Include(s => s.Employee)
            .AsNoTracking()
            .Where(s => s.Id == request.Id)
            .Select(s => new SalarySlipDto
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                EmployeeName = s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : "Unknown",
                Month = s.Month,
                BaseSalary = s.BaseSalary,
                Deductions = s.Deductions,
                NetPay = s.NetPay,
                Status = s.Status,
                CreatedAt = s.CreatedAt
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (slip is null)
            return ApiResponse<SalarySlipDto>.FailureResponse("Salary slip not found.");

        return ApiResponse<SalarySlipDto>.SuccessResponse(slip);
    }
}
