using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Commands;

public record ReleaseStockReservationCommand(
    Guid ProductId,
    Guid CompanyId,
    ReleaseStockReservationRequest Request)
    : IRequest<ReleaseStockReservationResponse>;