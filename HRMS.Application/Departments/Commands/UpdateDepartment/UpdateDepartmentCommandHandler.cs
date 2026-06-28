using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments.FindAsync(new object[] { request.Id }, cancellationToken);

        if (department == null)
        {
            return ApiResponse<Guid>.FailureResponse("Department not found.", new List<string> { $"Cannot find Department with Id {request.Id}" });
        }

        // Validate parent exists (if provided) and prevent self-parenting
        if (request.ParentDepartmentId.HasValue)
        {
            if (request.ParentDepartmentId.Value == request.Id)
            {
                return ApiResponse<Guid>.FailureResponse("Invalid parent department.", new List<string> { "Department cannot be its own parent." });
            }

            var parent = await _context.Departments.FindAsync(new object[] { request.ParentDepartmentId.Value }, cancellationToken);
            if (parent == null)
            {
                return ApiResponse<Guid>.FailureResponse("Parent department not found.", new List<string> { $"ParentDepartmentId {request.ParentDepartmentId.Value} does not exist." });
            }

            // prevent simple cycles: ensure parent chain doesn't include this department
            var cur = parent;
            var visited = new HashSet<Guid>();
            while (cur != null && cur.ParentDepartmentId.HasValue)
            {
                if (!visited.Add(cur.Id)) break;
                if (cur.ParentDepartmentId.Value == request.Id)
                {
                    return ApiResponse<Guid>.FailureResponse("Invalid parent relationship.", new List<string> { "Setting this parent would create a cycle." });
                }
                cur = await _context.Departments.FindAsync(new object[] { cur.ParentDepartmentId.Value }, cancellationToken);
            }
        }

        // Validate manager exists (if provided)
        if (request.ManagerId.HasValue)
        {
            var manager = await _context.Employees.FindAsync(new object[] { request.ManagerId.Value }, cancellationToken);
            if (manager == null)
            {
                return ApiResponse<Guid>.FailureResponse("Manager not found.", new List<string> { $"ManagerId {request.ManagerId.Value} does not exist." });
            }
        }

        department.Name = request.Name;
        department.Description = request.Description;
        department.ParentDepartmentId = request.ParentDepartmentId;
        department.ManagerId = request.ManagerId;
        department.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(department.Id, "Department updated successfully.");
    }
}
