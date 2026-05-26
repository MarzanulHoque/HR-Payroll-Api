using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public UpdateEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (employee == null)
        {
            return ApiResponse<Guid>.FailureResponse("Employee not found.");
        }

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Email = request.Email;
        employee.Department = request.Department;
        employee.Designation = request.Designation;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(employee.Id, "Employee updated successfully.");
    }
}
