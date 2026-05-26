using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Designations.Commands.UpdateDesignation;

public class UpdateDesignationCommandHandler : IRequestHandler<UpdateDesignationCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public UpdateDesignationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(UpdateDesignationCommand request, CancellationToken cancellationToken)
    {
        var designation = await _context.Designations.FindAsync(new object[] { request.Id }, cancellationToken);

        if (designation == null)
        {
            return ApiResponse<Guid>.FailureResponse("Designation not found.", new List<string> { $"Cannot find Designation with Id {request.Id}" });
        }

        designation.Title = request.Title;
        designation.Description = request.Description;
        designation.DepartmentId = request.DepartmentId;
        designation.IsActive = request.IsActive;
        designation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(designation.Id, "Designation updated successfully.");
    }
}
