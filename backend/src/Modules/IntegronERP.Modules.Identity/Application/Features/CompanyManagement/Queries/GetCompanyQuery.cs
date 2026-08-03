using IntegronERP.Modules.Identity.Application.Features.CompanyManagement.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.CompanyManagement.Queries;

public record GetCompanyQuery(Guid CompanyId)
    : IRequest<GetCompanyResponse>;