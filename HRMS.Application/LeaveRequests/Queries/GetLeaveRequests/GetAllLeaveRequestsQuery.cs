using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.LeaveRequests.Queries.GetLeaveRequests;

public record GetAllLeaveRequestsQuery : IRequest<ApiResponse<List<LeaveRequestDto>>>;
