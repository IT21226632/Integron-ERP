namespace IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;

public class LogoutResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;
}