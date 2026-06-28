using MediatR;
using HRMS.Application.Common.Models;

namespace HRMS.Application.Payroll.Commands.LockPayroll;

public record LockPayrollCommand(string Month) : IRequest<ApiResponse<bool>>;
