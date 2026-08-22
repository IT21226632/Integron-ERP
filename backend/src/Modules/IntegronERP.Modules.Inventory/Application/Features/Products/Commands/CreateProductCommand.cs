using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Commands;

public record CreateProductCommand(
    CreateProductRequest Request,
    Guid CompanyId)
    : IRequest<CreateProductResponse>;