using HRMS.Application.Auth.Commands.Login;
using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(
    string Token
) : IRequest<ApiResponse<AuthResponse>>;
