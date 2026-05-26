using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Designations.Commands.DeleteDesignation;

public class DeleteDesignationCommandHandler : IRequestHandler<DeleteDesignationCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public DeleteDesignationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
    {
        var designation = await _context.Designations.FindAsync(new object[] { request.Id }, cancellationToken);

        if (designation == null)
        {
            return ApiResponse<Guid>.FailureResponse("Designation not found.", new List<string> { $"Cannot find Designation with Id {request.Id}" });
        }

        _context.Designations.Remove(designation);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(designation.Id, "Designation deleted successfully.");
    }
}
