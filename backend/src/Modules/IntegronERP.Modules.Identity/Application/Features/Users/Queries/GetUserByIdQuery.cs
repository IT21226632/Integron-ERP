using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Queries;

public record GetUserByIdQuery(
    Guid UserId,
    Guid CompanyId)
    : IRequest<GetUserByIdResponse>;