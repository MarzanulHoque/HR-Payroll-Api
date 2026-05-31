using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Payroll.Queries.GetSalarySlips;

public record SalarySlipsQueryParameters
(
	string? Month,
	string? Search,
	string? SortBy,
	bool Desc = false,
	int Page = 1,
	int PageSize = 25
);

public record GetAllSalarySlipsQuery(SalarySlipsQueryParameters Parameters) : IRequest<ApiResponse<List<SalarySlipDto>>>;
