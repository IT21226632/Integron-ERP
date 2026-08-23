using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using IntegronERP.Modules.Inventory.Domain.Entities;
using MediatR;
using IntegronERP.Modules.Inventory.Domain.Constants;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Queries;

public record GetStockMovementsQuery(
    Guid ProductId,
    Guid CompanyId,
    int Page = 1,
    int PageSize = 20,
    StockMovementType? MovementType = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null)
    : IRequest<GetStockMovementsResponse>;