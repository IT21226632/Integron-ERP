using IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Authentication.Commands;

public class RefreshTokenCommand
    : IRequest<LoginResponse>
{
    public RefreshTokenRequest Request { get; }

    public RefreshTokenCommand(
        RefreshTokenRequest request)
    {
        Request = request;
    }
}