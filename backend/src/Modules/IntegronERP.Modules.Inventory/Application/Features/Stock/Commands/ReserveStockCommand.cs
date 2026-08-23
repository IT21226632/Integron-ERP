using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Commands;

public record ReserveStockCommand(
    Guid ProductId,
    Guid CompanyId,
    ReserveStockRequest Request)
    : IRequest<ReserveStockResponse>;