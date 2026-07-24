using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Commands;

public record UpdateUserStatusCommand(
    Guid UserId,
    Guid CompanyId,
    Guid CurrentUserId,
    UpdateUserStatusRequest Request)
    : IRequest<UpdateUserStatusResponse>;