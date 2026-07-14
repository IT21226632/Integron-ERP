using IntegronERP.Modules.Identity.Domain.Entities;

namespace IntegronERP.Modules.Identity.Application.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(ApplicationUser user, IList<string> roles);

    string GenerateRefreshToken();
}