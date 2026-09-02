using IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Suppliers.Queries;

public record GetSuppliersQuery(
    bool ActiveOnly
) : IRequest<GetSuppliersResponse>;