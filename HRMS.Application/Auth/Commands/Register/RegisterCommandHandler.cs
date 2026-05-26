using HRMS.Application.Common.Interfaces;
using HRMS.Application.Common.Models;
using HRMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponse<Guid>>
{
    private readonly IApplicationDbContext _context;

    public RegisterCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Check if user exists
        if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            return ApiResponse<Guid>.FailureResponse("User with this email already exists.");
        }

        // 2. Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 3. Create User
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash
        };

        // 4. Save
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(user.Id, "User registered successfully.");
    }
}
