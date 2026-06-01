using MediatR;
using System;
using HRMS.Application.Common.Models;

namespace HRMS.Application.Payroll.Commands.AddPayrollAdjustment;

public record AddPayrollAdjustmentCommand(Guid SalarySlipId, decimal Amount, string Reason) : IRequest<ApiResponse<Guid>>;
