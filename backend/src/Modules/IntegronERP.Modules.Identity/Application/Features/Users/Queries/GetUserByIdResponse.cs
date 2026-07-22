using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Queries;

public class GetUserByIdResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public UserDetailsDto? User { get; set; }
}