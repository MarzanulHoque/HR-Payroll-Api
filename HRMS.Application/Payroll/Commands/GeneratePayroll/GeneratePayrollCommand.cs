using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Payroll.Commands.GeneratePayroll;

public record GeneratePayrollCommand(
    Guid EmployeeId,
    string Month,
    decimal BaseSalary,
    decimal Deductions
) : IRequest<ApiResponse<Guid>>;
