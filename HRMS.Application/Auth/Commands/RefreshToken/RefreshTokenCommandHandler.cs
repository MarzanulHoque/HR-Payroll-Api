using HRMS.Application.Auth.Commands.Login;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Interfaces.Auth;
using HRMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ApiResponse<AuthResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(IApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1. Find the active refresh token in the DB and include User data
        var existingToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(rt => rt.Token == request.Token, cancellationToken);

        if (existingToken == null || !existingToken.IsActive)
        {
            return ApiResponse<AuthResponse>.FailureResponse("Invalid or expired refresh token.");
        }

        // 2. Revoke old token
        existingToken.Revoked = DateTime.UtcNow;

        // 3. Generate new tokens
        var roles = existingToken.User.UserRoles.Select(ur => ur.Role.Name).ToList();
        var accessToken = _tokenService.GenerateAccessToken(existingToken.User, roles);
        var newRefreshTokenString = _tokenService.GenerateRefreshToken();

        // 4. Save new refresh token
        var newRefreshToken = new Domain.Entities.RefreshToken
        {
            Token = newRefreshTokenString,
            Expires = DateTime.UtcNow.AddDays(7),
            UserId = existingToken.UserId
        };
        
        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 5. Return success
        return ApiResponse<AuthResponse>.SuccessResponse(new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshTokenString
        }, "Token refreshed successfully.");
    }
}
