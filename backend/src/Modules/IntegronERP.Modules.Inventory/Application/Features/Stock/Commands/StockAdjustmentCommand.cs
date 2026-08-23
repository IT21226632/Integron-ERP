using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Commands;

public record StockAdjustmentCommand(
    Guid ProductId,
    Guid CompanyId,
    StockAdjustmentRequest Request)
    : IRequest<StockAdjustmentResponse>;