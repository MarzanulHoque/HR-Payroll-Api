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
        // 1. Find the refresh token in the DB and include User data
        var existingToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(rt => rt.Token == request.Token, cancellationToken);

        if (existingToken == null)
        {
            return ApiResponse<AuthResponse>.FailureResponse("Invalid or expired refresh token.");
        }

        // 1.a If the token is not active, detect reuse: if it was revoked and has a replacement,
        // treat as a possible token reuse attack and revoke all active tokens for the user.
        if (!existingToken.IsActive)
        {
            if (existingToken.Revoked != null && !string.IsNullOrEmpty(existingToken.ReplacedByToken))
            {
                // Revoke all active refresh tokens for this user
                var userActiveTokens = _context.RefreshTokens.Where(t => t.UserId == existingToken.UserId && t.Revoked == null);
                await userActiveTokens.ForEachAsync(t => t.Revoked = DateTime.UtcNow, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return ApiResponse<AuthResponse>.FailureResponse("Refresh token reuse detected. All user sessions revoked.");
            }

            return ApiResponse<AuthResponse>.FailureResponse("Invalid or expired refresh token.");
        }

        // 2. Generate new tokens
        var roles = existingToken.User.UserRoles.Select(ur => ur.Role.Name).ToList();
        var accessToken = _tokenService.GenerateAccessToken(existingToken.User, roles);
        var newRefreshTokenString = _tokenService.GenerateRefreshToken();

        // 3. Revoke old token and mark replacement
        existingToken.Revoked = DateTime.UtcNow;
        existingToken.ReplacedByToken = newRefreshTokenString;

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
