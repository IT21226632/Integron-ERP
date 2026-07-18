namespace IntegronERP.Modules.Identity.Application.Features.Users.DTOs;

public class CreateUserResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid? UserId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}