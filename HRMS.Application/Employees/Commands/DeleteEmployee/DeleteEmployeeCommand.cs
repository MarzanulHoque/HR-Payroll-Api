using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(Guid Id) : IRequest<ApiResponse<Guid>>;
