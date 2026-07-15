using Microsoft.AspNetCore.Identity;

namespace IntegronERP.Modules.Identity.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    // Tenant relationship
    public Guid CompanyId { get; set; }

    public Company Company { get; set; } = null!;


    // User profile information
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;


    // Account status
    public bool IsActive { get; set; } = true;


    // Audit information
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAt { get; set; }

      // Authentication
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}