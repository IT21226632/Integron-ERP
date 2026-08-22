using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Queries;

public record GetProductsQuery(
    Guid CompanyId,
    bool ActiveOnly = false)
    : IRequest<GetProductsResponse>;