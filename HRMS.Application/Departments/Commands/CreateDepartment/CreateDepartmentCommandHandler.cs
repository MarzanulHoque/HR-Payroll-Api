using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        // Validate parent exists (if provided)
        if (request.ParentDepartmentId.HasValue)
        {
            var parent = await _context.Departments.FindAsync(new object[] { request.ParentDepartmentId.Value }, cancellationToken);
            if (parent == null)
            {
                return ApiResponse<Guid>.FailureResponse("Parent department not found.", new List<string> { $"ParentDepartmentId {request.ParentDepartmentId.Value} does not exist." });
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

        var department = new Department
        {
            Name = request.Name,
            Description = request.Description,
            ParentDepartmentId = request.ParentDepartmentId,
            ManagerId = request.ManagerId
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(department.Id, "Department created successfully.");
    }
}
