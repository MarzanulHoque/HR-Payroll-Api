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
        // Simple mapping without AutoMapper for now
        var employees = await _context.Employees
            .Where(e => e.IsActive)
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
