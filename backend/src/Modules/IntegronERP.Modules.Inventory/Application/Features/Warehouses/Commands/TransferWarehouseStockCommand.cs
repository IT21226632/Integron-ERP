using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

public record TransferWarehouseStockCommand(
    Guid ProductId,
    Guid CompanyId,
    TransferWarehouseStockRequest Request)
    : IRequest<TransferWarehouseStockResponse>;