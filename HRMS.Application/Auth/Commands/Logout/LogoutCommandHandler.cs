using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _context;

    public LogoutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == request.UserId && rt.IsActive)
            .ToListAsync(cancellationToken);

        if (!tokens.Any())
        {
            return ApiResponse<bool>.SuccessResponse(true, "No active refresh tokens found.");
        }

        foreach (var t in tokens)
        {
            t.Revoked = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.SuccessResponse(true, "Logged out and refresh tokens revoked.");
    }
}
