using IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Authentication.Commands;

public class LoginCommand : IRequest<LoginResponse>
{
    public LoginRequest Request { get; }

    public LoginCommand(LoginRequest request)
    {
        Request = request;
    }
}