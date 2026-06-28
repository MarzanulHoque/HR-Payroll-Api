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
        var q = _context.SalarySlips
            .Include(s => s.Employee)
            .AsNoTracking()
            .AsQueryable();

        var p = request.Parameters;
        if (!string.IsNullOrWhiteSpace(p.Month))
            q = q.Where(s => s.Month == p.Month);

        if (!string.IsNullOrWhiteSpace(p.Search))
        {
            var s = p.Search.ToLower();
            q = q.Where(x => (x.Employee != null && (x.Employee.FirstName + " " + x.Employee.LastName).ToLower().Contains(s)) || x.Month.ToLower().Contains(s));
        }

        // Sorting
        q = (p.SortBy?.ToLower()) switch
        {
            "basessalary" => p.Desc ? q.OrderByDescending(x => x.BaseSalary) : q.OrderBy(x => x.BaseSalary),
            "netpay" => p.Desc ? q.OrderByDescending(x => x.NetPay) : q.OrderBy(x => x.NetPay),
            "createdat" => p.Desc ? q.OrderByDescending(x => x.CreatedAt) : q.OrderBy(x => x.CreatedAt),
            _ => p.Desc ? q.OrderByDescending(x => x.CreatedAt) : q.OrderBy(x => x.CreatedAt)
        };

        // Pagination
        var skip = (Math.Max(p.Page, 1) - 1) * Math.Clamp(p.PageSize, 1, 100);
        var items = await q.Skip(skip).Take(p.PageSize)
            .Select(s => new SalarySlipDto
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                EmployeeName = s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : "Unknown",
                EmployeeEmail = s.Employee != null ? s.Employee.Email : string.Empty,
                Month = s.Month,
                BaseSalary = s.BaseSalary,
                Deductions = s.Deductions,
                NetPay = s.NetPay,
                Status = s.Status,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<SalarySlipDto>>.SuccessResponse(items);
    }
}
