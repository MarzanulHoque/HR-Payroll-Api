using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Designations.Queries.GetDesignationById;

public class GetDesignationByIdQueryHandler : IRequestHandler<GetDesignationByIdQuery, ApiResponse<DesignationDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDesignationByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<DesignationDto>> Handle(GetDesignationByIdQuery request, CancellationToken cancellationToken)
    {
        var designation = await _context.Designations
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            
        if (designation == null)
        {
            return ApiResponse<DesignationDto>.FailureResponse("Designation not found.");
        }

        var dto = new DesignationDto
        {
            Id = designation.Id,
            Title = designation.Title,
            Description = designation.Description,
            DepartmentId = designation.DepartmentId,
            IsActive = designation.IsActive,
            CreatedAt = designation.CreatedAt
        };

        return ApiResponse<DesignationDto>.SuccessResponse(dto);
    }
}
