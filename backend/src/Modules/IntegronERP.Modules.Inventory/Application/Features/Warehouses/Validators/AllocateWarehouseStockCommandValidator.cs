using FluentValidation;
using IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Validators;

public class AllocateWarehouseStockCommandValidator
    : AbstractValidator<AllocateWarehouseStockCommand>
{
    public AllocateWarehouseStockCommandValidator()
    {
        RuleFor(x => x.Request.WarehouseId)
            .NotEmpty()
            .WithMessage("Warehouse is required.");

        RuleFor(x => x.Request.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}