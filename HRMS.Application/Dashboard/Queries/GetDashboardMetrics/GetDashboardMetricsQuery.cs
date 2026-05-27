using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Dashboard.Queries.GetDashboardMetrics;

public record GetDashboardMetricsQuery : IRequest<ApiResponse<DashboardMetricsDto>>;
