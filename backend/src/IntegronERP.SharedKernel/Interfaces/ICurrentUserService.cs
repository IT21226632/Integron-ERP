using System.Security.Claims;

namespace IntegronERP.SharedKernel.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }

    Guid UserId { get; }

    Guid CompanyId { get; }

    string Email { get; }

    string UserName { get; }

    string Role { get; }

    ClaimsPrincipal User { get; }
}