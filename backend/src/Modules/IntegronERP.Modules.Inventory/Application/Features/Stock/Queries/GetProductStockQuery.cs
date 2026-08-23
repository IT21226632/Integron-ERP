using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Queries;

public record GetProductStockQuery(
    Guid ProductId,
    Guid CompanyId)
    : IRequest<GetProductStockResponse>;