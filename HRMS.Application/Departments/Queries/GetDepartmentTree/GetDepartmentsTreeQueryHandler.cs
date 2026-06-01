using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Departments.Queries.GetDepartmentTree;

public class GetDepartmentsTreeQueryHandler : IRequestHandler<GetDepartmentsTreeQuery, ApiResponse<List<DepartmentTreeDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetDepartmentsTreeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<DepartmentTreeDto>>> Handle(GetDepartmentsTreeQuery request, CancellationToken cancellationToken)
    {
        var depts = await _context.Departments
            .AsNoTracking()
            .Select(d => new
            {
                Dept = d,
                Manager = d.Manager == null ? null : new { d.Manager.Id, d.Manager.FirstName, d.Manager.LastName, d.Manager.Email }
            })
            .ToListAsync(cancellationToken);

        var deptDtos = depts.Select(x => new DepartmentTreeDto
        {
            Id = x.Dept.Id,
            Name = x.Dept.Name,
            Description = x.Dept.Description,
            ManagerId = x.Dept.ManagerId,
            Manager = x.Manager == null ? null : new HRMS.Application.Common.Dtos.EmployeeMiniDto { Id = x.Manager.Id, FirstName = x.Manager.FirstName, LastName = x.Manager.LastName, Email = x.Manager.Email },
            CreatedAt = x.Dept.CreatedAt
        }).ToList();

        var lookup = deptDtos.ToDictionary(d => d.Id);
        var roots = new List<DepartmentTreeDto>();

        // Need ParentDepartmentId values, fetch separately
        var parentMap = await _context.Departments.AsNoTracking().Select(d => new { d.Id, d.ParentDepartmentId }).ToListAsync(cancellationToken);
        var parentLookup = parentMap.ToDictionary(x => x.Id, x => x.ParentDepartmentId);

        foreach (var dto in deptDtos)
        {
            if (parentLookup.TryGetValue(dto.Id, out var parentId) && parentId.HasValue && lookup.TryGetValue(parentId.Value, out var parent))
            {
                parent.Children.Add(dto);
            }
            else
            {
                roots.Add(dto);
            }
        }

        return ApiResponse<List<DepartmentTreeDto>>.SuccessResponse(roots);
    }
}
