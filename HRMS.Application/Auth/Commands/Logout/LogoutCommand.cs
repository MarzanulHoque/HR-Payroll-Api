using HRMS.Application.Common.Models;
using MediatR;

namespace HRMS.Application.Auth.Commands.Logout;

public record LogoutCommand(
    Guid UserId
) : IRequest<ApiResponse<bool>>;
