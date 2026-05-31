using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, ApiResponse<List<EmployeeDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<EmployeeDto>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        // Basic pagination support. Search/sort will be added later.
        var q = _context.Employees
            .Where(e => e.IsActive)
            .AsNoTracking()
            .AsQueryable();

        var page = Math.Max(request.Page, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);
        var skip = (page - 1) * pageSize;

        var employees = await q
            .Skip(skip)
            .Take(pageSize)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Department = e.Department,
                Designation = e.Designation,
                IsActive = e.IsActive
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<EmployeeDto>>.SuccessResponse(employees, "Employees retrieved successfully.");
    }
}
