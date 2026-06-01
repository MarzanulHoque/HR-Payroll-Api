using MediatR;
using System;

namespace HRMS.Application.Payroll.Commands.AddSalaryIncrement;

public record AddSalaryIncrementCommand(Guid EmployeeId, decimal Amount, DateTime EffectiveFrom, string Reason) : IRequest<ApiResponse<Guid>>;
