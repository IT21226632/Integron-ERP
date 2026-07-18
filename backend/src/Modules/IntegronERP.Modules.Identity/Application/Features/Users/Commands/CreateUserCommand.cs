using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Commands;

public record CreateUserCommand(
    CreateUserRequest Request,
    Guid CompanyId)
    : IRequest<CreateUserResponse>;