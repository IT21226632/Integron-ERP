using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Commands;

public record ResetPasswordCommand(
    Guid UserId,
    Guid CurrentUserId,
    Guid CompanyId,
    ResetPasswordRequest Request)
    : IRequest<ResetPasswordResponse>;