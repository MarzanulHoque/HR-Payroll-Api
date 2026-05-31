namespace HRMS.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Revoked { get; set; }
    // Token string of the refresh token that replaced this one (if rotated)
    public string? ReplacedByToken { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
