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
    private readonly IAuditService _auditService;

    public LoginCommandHandler(IApplicationDbContext context, ITokenService tokenService, IAuditService auditService)
    {
        _context = context;
        _tokenService = tokenService;
        _auditService = auditService;
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
            await _auditService.RecordAsync("LoginFailed", "User", null, request.Email, null, null, null);
            return ApiResponse<AuthResponse>.FailureResponse("Invalid credentials or inactive account.");
        }

        // 2. Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            await _auditService.RecordAsync("LoginFailed", "User", user.Id, request.Email, null, null, null);
            return ApiResponse<AuthResponse>.FailureResponse("Invalid credentials.");
        }

        // 3. Generate tokens
        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshTokenString = _tokenService.GenerateRefreshToken();

        // 4. Save refresh token
        var refreshToken = new Domain.Entities.RefreshToken
        {
            TokenHash = HashToken(refreshTokenString),
            Expires = DateTime.UtcNow.AddDays(7), // Set default expiry logic here or via config
            UserId = user.Id
        };
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        // record successful login
        await _auditService.RecordAsync("LoginSuccess", "User", user.Id, user.Email, null, null, null);

        // 5. Response
        var authResponse = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenString
        };

        return ApiResponse<AuthResponse>.SuccessResponse(authResponse, "Login successful.");
    }

    private static string HashToken(string token)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}
