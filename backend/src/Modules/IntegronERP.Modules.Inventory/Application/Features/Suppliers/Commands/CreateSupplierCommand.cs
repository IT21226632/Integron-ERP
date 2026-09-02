using IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Suppliers.Commands;

public record CreateSupplierCommand(
    string Name,
    string? Email,
    string? PhoneNumber,
    string? ContactPerson,
    string? Address
) : IRequest<CreateSupplierResponse>;