using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Payroll.Queries.GetSalarySlips;

public record GetAllSalarySlipsQuery : IRequest<ApiResponse<List<SalarySlipDto>>>;
