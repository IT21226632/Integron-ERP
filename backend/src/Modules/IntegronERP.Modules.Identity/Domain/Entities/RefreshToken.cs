using System.ComponentModel.DataAnnotations;

namespace IntegronERP.Modules.Identity.Domain.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool Revoked { get; set; }

    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    public bool IsExpired =>
        DateTime.UtcNow >= ExpiresAt;

    public bool IsActive =>
        !Revoked && !IsExpired;
}