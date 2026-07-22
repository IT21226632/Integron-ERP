using System.Security.Claims;
using IntegronERP.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace IntegronERP.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal User =>
        _httpContextAccessor.HttpContext?.User
        ?? new ClaimsPrincipal();

    public bool IsAuthenticated =>
        User.Identity?.IsAuthenticated ?? false;

    public Guid UserId =>
        Guid.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out var id)
            ? id
            : Guid.Empty;

    public Guid CompanyId =>
        Guid.TryParse(
            User.FindFirst("CompanyId")?.Value,
            out var id)
            ? id
            : Guid.Empty;

    public string Email =>
        User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

    public string UserName =>
        User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

    public string Role =>
        User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

    ClaimsPrincipal ICurrentUserService.User => User;
}