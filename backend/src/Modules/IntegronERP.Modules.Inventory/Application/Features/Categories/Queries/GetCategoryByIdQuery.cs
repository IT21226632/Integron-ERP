using IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Categories.Queries;

public record GetCategoryByIdQuery(
    Guid Id,
    Guid CompanyId
) : IRequest<GetCategoryByIdResponse>;