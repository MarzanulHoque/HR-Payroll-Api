using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;

namespace HRMS.Application.Designations.Commands.CreateDesignation;

public class CreateDesignationCommandHandler : IRequestHandler<CreateDesignationCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateDesignationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateDesignationCommand request, CancellationToken cancellationToken)
    {
        var designation = new Designation
        {
            Title = request.Title,
            Description = request.Description,
            DepartmentId = request.DepartmentId,
            IsActive = request.IsActive
        };

        _context.Designations.Add(designation);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(designation.Id, "Designation created successfully.");
    }
}
