namespace HRMS.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    // Stored as SHA-256 hex of the actual refresh token string
    public string TokenHash { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Revoked { get; set; }
    // SHA-256 hex of the token that replaced this one (if rotated)
    public string? ReplacedByTokenHash { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
