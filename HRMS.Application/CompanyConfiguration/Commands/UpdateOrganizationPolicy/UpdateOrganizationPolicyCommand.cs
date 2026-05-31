using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.CompanyConfiguration.Commands.UpdateOrganizationPolicy;

public record UpdateOrganizationPolicyCommand(
    string OrganizationName,
    string WorkingDays,
    string OfficeHours,
    string TimeZone,
    string CurrencyCode,
    string AttendancePolicy,
    string LeavePolicy,
    string PayrollPolicy,
    bool IsActive) : IRequest<ApiResponse<OrganizationPolicyDto>>;