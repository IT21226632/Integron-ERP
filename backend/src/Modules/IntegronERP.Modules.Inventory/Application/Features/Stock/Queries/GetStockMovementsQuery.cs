using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Queries;

public record GetStockMovementsQuery(
    Guid ProductId,
    Guid CompanyId)
    : IRequest<GetStockMovementsResponse>;