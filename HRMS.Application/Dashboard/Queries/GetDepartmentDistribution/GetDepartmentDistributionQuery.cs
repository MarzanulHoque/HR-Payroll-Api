using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Dashboard.Queries.GetDepartmentDistribution;

public record GetDepartmentDistributionQuery : IRequest<ApiResponse<List<DepartmentDistributionPointDto>>>;
