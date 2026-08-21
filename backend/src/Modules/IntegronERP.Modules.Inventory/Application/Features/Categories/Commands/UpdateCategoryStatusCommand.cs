using IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Categories.Commands;

public record UpdateCategoryStatusCommand(
    Guid Id,
    Guid CompanyId,
    UpdateCategoryStatusRequest Request
) : IRequest<UpdateCategoryStatusResponse>;