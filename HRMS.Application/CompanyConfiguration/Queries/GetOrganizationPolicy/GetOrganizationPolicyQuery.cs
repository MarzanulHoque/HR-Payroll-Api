using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.CompanyConfiguration.Queries.GetOrganizationPolicy;

public record GetOrganizationPolicyQuery() : IRequest<ApiResponse<OrganizationPolicyDto>>;