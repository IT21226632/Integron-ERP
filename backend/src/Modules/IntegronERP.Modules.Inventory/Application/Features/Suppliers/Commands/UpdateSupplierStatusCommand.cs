using IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Suppliers.Commands;

public record UpdateSupplierStatusCommand(
    Guid Id,
    bool IsActive
) : IRequest<UpdateSupplierStatusResponse?>;