using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Queries;

public record GetProductByIdQuery(
    Guid ProductId,
    Guid CompanyId)
    : IRequest<GetProductByIdResponse>;