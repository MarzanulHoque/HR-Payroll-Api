using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.CompanyConfiguration.Queries.GetOrganizationPolicy;

public class GetOrganizationPolicyQueryHandler : IRequestHandler<GetOrganizationPolicyQuery, ApiResponse<OrganizationPolicyDto>>
{
    private readonly IApplicationDbContext _context;

    public GetOrganizationPolicyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<OrganizationPolicyDto>> Handle(GetOrganizationPolicyQuery request, CancellationToken cancellationToken)
    {
        var policy = await _context.OrganizationPolicies
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        policy ??= new OrganizationPolicy();

        return ApiResponse<OrganizationPolicyDto>.SuccessResponse(Map(policy), "Organization policy retrieved successfully.");
    }

    private static OrganizationPolicyDto Map(OrganizationPolicy policy)
    {
        return new OrganizationPolicyDto
        {
            Id = policy.Id,
            OrganizationName = policy.OrganizationName,
            WorkingDays = policy.WorkingDays,
            OfficeHours = policy.OfficeHours,
            TimeZone = policy.TimeZone,
            CurrencyCode = policy.CurrencyCode,
            AttendancePolicy = policy.AttendancePolicy,
            LeavePolicy = policy.LeavePolicy,
            PayrollPolicy = policy.PayrollPolicy,
            IsActive = policy.IsActive
        };
    }
}