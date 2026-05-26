using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Auth.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<ApiResponse<AuthResponse>>;
