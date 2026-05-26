using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Auth.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<ApiResponse<Guid>>;
