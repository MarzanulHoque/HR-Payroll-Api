using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Dashboard.Queries.GetAttendanceTrends;

public record GetAttendanceTrendsQuery(string? Month) : IRequest<ApiResponse<List<AttendanceTrendPointDto>>>;
