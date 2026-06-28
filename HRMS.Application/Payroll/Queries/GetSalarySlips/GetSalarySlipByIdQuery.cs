using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Payroll.Queries.GetSalarySlips;

public record GetSalarySlipByIdQuery(Guid Id) : IRequest<ApiResponse<SalarySlipDto>>;
