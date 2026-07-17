namespace IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;

public class LogoutRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}