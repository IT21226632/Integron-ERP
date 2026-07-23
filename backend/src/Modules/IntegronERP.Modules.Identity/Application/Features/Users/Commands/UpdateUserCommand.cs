using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Commands;

public record UpdateUserCommand(
    Guid UserId,
    Guid CompanyId,
    UpdateUserRequest Request)
    : IRequest<UpdateUserResponse>;