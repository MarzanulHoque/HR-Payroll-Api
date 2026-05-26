using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Application.Departments.Queries.GetDepartmentById;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HRMS.Application.Departments.Queries.GetAllDepartments;

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, ApiResponse<List<DepartmentDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllDepartmentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<DepartmentDto>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await _context.Departments
            .AsNoTracking()
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                ParentDepartmentId = d.ParentDepartmentId,
                ManagerId = d.ManagerId,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<DepartmentDto>>.SuccessResponse(departments);
    }
}
