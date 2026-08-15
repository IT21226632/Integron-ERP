using IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Categories.Commands;

public record CreateCategoryCommand(
    CreateCategoryRequest Request,
    Guid CompanyId) : IRequest<CreateCategoryResponse>;