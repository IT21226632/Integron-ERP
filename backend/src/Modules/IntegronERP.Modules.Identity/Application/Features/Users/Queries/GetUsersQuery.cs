using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Queries;

public record GetUsersQuery(Guid CompanyId)
    : IRequest<GetUsersResponse>;