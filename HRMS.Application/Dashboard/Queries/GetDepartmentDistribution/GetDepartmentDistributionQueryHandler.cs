using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Dashboard.Queries.GetDepartmentDistribution;

public class GetDepartmentDistributionQueryHandler : IRequestHandler<GetDepartmentDistributionQuery, ApiResponse<List<DepartmentDistributionPointDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetDepartmentDistributionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<DepartmentDistributionPointDto>>> Handle(GetDepartmentDistributionQuery request, CancellationToken cancellationToken)
    {
        var departments = await _context.Departments
            .AsNoTracking()
            .Select(d => new
            {
                d.Name,
                EmployeeCount = _context.Employees.Count(e => e.Department == d.Name)
            })
            .ToListAsync(cancellationToken);

        var points = departments
            .Select(d => new DepartmentDistributionPointDto
            {
                Label = d.Name,
                Value = d.EmployeeCount
            })
            .OrderByDescending(x => x.Value)
            .ToList();

        return ApiResponse<List<DepartmentDistributionPointDto>>.SuccessResponse(points);
    }
}
