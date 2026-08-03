using IntegronERP.Modules.Identity.Application.Features.CompanyManagement.DTOs;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.CompanyManagement.Commands;

public record UpdateCompanyCommand(
    Guid CompanyId,
    UpdateCompanyRequest Request)
    : IRequest<UpdateCompanyResponse>;