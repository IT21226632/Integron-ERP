using System.ComponentModel.DataAnnotations;

namespace IntegronERP.Modules.Identity.Domain.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    // Revocation status
    public bool Revoked { get; set; }

    public DateTime? RevokedAt { get; set; }

    // User relationship
    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    // Computed properties
    public bool IsExpired =>
        DateTime.UtcNow >= ExpiresAt;

    public bool IsRevoked =>
        Revoked || RevokedAt.HasValue;

    public bool IsActive =>
        !IsRevoked && !IsExpired;
}