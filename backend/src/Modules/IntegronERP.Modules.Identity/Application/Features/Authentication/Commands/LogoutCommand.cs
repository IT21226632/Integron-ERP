using IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Authentication.Commands;

public record LogoutCommand(
    LogoutRequest Request)
    : IRequest<LogoutResponse>;