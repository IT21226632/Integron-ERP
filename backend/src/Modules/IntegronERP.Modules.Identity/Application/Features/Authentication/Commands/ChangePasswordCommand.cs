using IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Authentication.Commands;

public record ChangePasswordCommand(
    Guid UserId,
    ChangePasswordRequest Request)
    : IRequest<ChangePasswordResponse>;