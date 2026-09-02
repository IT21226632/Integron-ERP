using IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Suppliers.Commands;

public record UpdateSupplierCommand(
    Guid Id,
    string Name,
    string? Email,
    string? PhoneNumber,
    string? ContactPerson,
    string? Address
) : IRequest<UpdateSupplierResponse?>;