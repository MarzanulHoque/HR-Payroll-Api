using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Application.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, ApiResponse<EmployeeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeeByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<EmployeeDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .Where(e => e.Id == request.Id && e.IsActive)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (employee == null)
        {
            return ApiResponse<EmployeeDto>.FailureResponse("Employee not found.");
        }

        return ApiResponse<EmployeeDto>.SuccessResponse(employee, "Employee retrieved successfully.");
    }
}
