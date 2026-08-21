using IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Categories.Queries;

public record GetCategoriesQuery(
    Guid CompanyId,
    bool ActiveOnly = false
) : IRequest<GetCategoriesResponse>;