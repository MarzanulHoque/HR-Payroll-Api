using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Payroll.Queries.GetSalarySlips;

public class GetAllSalarySlipsQueryHandler : IRequestHandler<GetAllSalarySlipsQuery, ApiResponse<List<SalarySlipDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllSalarySlipsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<SalarySlipDto>>> Handle(GetAllSalarySlipsQuery request, CancellationToken cancellationToken)
    {
        var slips = await _context.SalarySlips
            .Include(s => s.Employee)
            .AsNoTracking()
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
            .ToListAsync(cancellationToken);

        return ApiResponse<List<SalarySlipDto>>.SuccessResponse(slips);
    }
}
