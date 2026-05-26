using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Application.Designations.Queries.GetDesignationById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Designations.Queries.GetAllDesignations;

public class GetAllDesignationsQueryHandler : IRequestHandler<GetAllDesignationsQuery, ApiResponse<List<DesignationDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllDesignationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<DesignationDto>>> Handle(GetAllDesignationsQuery request, CancellationToken cancellationToken)
    {
        var designations = await _context.Designations
            .AsNoTracking()
            .Select(d => new DesignationDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                DepartmentId = d.DepartmentId,
                IsActive = d.IsActive,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<DesignationDto>>.SuccessResponse(designations);
    }
}
