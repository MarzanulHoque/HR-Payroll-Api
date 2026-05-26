using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public DeleteEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (employee == null)
        {
            return ApiResponse<Guid>.FailureResponse("Employee not found.");
        }

        // Soft delete mapped from requirements
        employee.IsActive = false;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(employee.Id, "Employee deleted successfully.");
    }
}
