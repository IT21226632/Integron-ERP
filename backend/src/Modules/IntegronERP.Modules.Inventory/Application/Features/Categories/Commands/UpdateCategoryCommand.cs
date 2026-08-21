using IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Categories.Commands;

public record UpdateCategoryCommand(
    Guid Id,
    Guid CompanyId,
    UpdateCategoryRequest Request
) : IRequest<UpdateCategoryResponse>;