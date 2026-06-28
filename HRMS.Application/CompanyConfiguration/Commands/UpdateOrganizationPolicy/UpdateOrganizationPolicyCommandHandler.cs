using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.CompanyConfiguration.Commands.UpdateOrganizationPolicy;

public class UpdateOrganizationPolicyCommandHandler : IRequestHandler<UpdateOrganizationPolicyCommand, ApiResponse<OrganizationPolicyDto>>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrganizationPolicyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<OrganizationPolicyDto>> Handle(UpdateOrganizationPolicyCommand request, CancellationToken cancellationToken)
    {
        var policy = await _context.OrganizationPolicies.FirstOrDefaultAsync(cancellationToken);

        if (policy is null)
        {
            policy = new OrganizationPolicy();
            _context.OrganizationPolicies.Add(policy);
        }

        policy.OrganizationName = request.OrganizationName;
        policy.WorkingDays = request.WorkingDays;
        policy.OfficeHours = request.OfficeHours;
        policy.TimeZone = request.TimeZone;
        policy.CurrencyCode = request.CurrencyCode;
        policy.AttendancePolicy = request.AttendancePolicy;
        policy.LeavePolicy = request.LeavePolicy;
        policy.PayrollPolicy = request.PayrollPolicy;
        policy.IsActive = request.IsActive;
        policy.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<OrganizationPolicyDto>.SuccessResponse(new OrganizationPolicyDto
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
        }, "Organization policy updated successfully.");
    }
}