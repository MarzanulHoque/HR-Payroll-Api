using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Departments.Queries.GetDepartments;

public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, ApiResponse<List<DepartmentDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetDepartmentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<DepartmentDto>>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await _context.Departments
            .Where(d => d.IsActive)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                ParentDepartmentId = d.ParentDepartmentId,
                ManagerId = d.ManagerId,
                IsActive = d.IsActive
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<DepartmentDto>>.SuccessResponse(departments, "Departments retrieved successfully.");
    }
}
