using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Commands;

public record UpdateProductStatusCommand(
    Guid ProductId,
    Guid CompanyId,
    UpdateProductStatusRequest Request)
    : IRequest<UpdateProductStatusResponse>;