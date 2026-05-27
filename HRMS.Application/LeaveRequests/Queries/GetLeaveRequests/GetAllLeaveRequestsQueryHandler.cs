using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.LeaveRequests.Queries.GetLeaveRequests;

public class GetAllLeaveRequestsQueryHandler : IRequestHandler<GetAllLeaveRequestsQuery, ApiResponse<List<LeaveRequestDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllLeaveRequestsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<LeaveRequestDto>>> Handle(GetAllLeaveRequestsQuery request, CancellationToken cancellationToken)
    {
        var requests = await _context.LeaveRequests
            .Include(lr => lr.Employee)
            .AsNoTracking()
            .Select(lr => new LeaveRequestDto
            {
                Id = lr.Id,
                EmployeeId = lr.EmployeeId,
                EmployeeName = lr.Employee != null ? $"{lr.Employee.FirstName} {lr.Employee.LastName}" : "Unknown",
                LeaveType = lr.LeaveType,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                Reason = lr.Reason,
                Status = lr.Status,
                CreatedAt = lr.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<LeaveRequestDto>>.SuccessResponse(requests);
    }
}
