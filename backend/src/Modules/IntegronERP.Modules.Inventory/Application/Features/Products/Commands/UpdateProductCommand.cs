using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Commands;

public record UpdateProductCommand(
    Guid ProductId,
    Guid CompanyId,
    UpdateProductRequest Request)
    : IRequest<UpdateProductResponse>;