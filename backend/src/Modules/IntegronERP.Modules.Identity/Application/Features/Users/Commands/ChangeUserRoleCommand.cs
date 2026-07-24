using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Commands;

public record ChangeUserRoleCommand(
    Guid UserId,
    Guid CurrentUserId,
    Guid CompanyId,
    ChangeUserRoleRequest Request)
    : IRequest<ChangeUserRoleResponse>;