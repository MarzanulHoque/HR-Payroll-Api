using MediatR;

namespace HRMS.Application.Payroll.Commands.LockPayroll;

public record LockPayrollCommand(string Month) : IRequest<ApiResponse<bool>>;
