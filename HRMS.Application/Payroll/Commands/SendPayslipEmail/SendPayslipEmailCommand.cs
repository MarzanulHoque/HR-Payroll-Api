using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Payroll.Commands.SendPayslipEmail;

public record SendPayslipEmailCommand(Guid SalarySlipId, string ToEmail) : IRequest<ApiResponse<bool>>;
