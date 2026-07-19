namespace IntegronERP.Modules.Identity.Application.Features.Users.DTOs;

public class GetUsersResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<UserDto> Users { get; set; } = [];
}