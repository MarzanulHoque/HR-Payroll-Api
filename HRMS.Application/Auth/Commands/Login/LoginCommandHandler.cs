using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Interfaces.Auth;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<AuthResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Find user
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null || !user.IsActive)
        {
            return ApiResponse<AuthResponse>.FailureResponse("Invalid credentials or inactive account.");
        }

        // 2. Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return ApiResponse<AuthResponse>.FailureResponse("Invalid credentials.");
        }

        // 3. Generate tokens
        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshTokenString = _tokenService.GenerateRefreshToken();

        // 4. Save refresh token
        var refreshToken = new Domain.Entities.RefreshToken
        {
            Token = refreshTokenString,
            Expires = DateTime.UtcNow.AddDays(7), // Set default expiry logic here or via config
            UserId = user.Id
        };
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 5. Response
        var authResponse = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenString
        };

        return ApiResponse<AuthResponse>.SuccessResponse(authResponse, "Login successful.");
    }
}
