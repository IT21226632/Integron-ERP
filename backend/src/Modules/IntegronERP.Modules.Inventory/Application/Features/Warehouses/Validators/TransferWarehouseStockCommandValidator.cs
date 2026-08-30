using FluentValidation;
using IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Validators;

public class TransferWarehouseStockCommandValidator
    : AbstractValidator<TransferWarehouseStockCommand>
{
    public TransferWarehouseStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product is required.");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("Company information is required.");

        RuleFor(x => x.Request.FromWarehouseId)
            .NotEmpty()
            .WithMessage("Source warehouse is required.");

        RuleFor(x => x.Request.ToWarehouseId)
            .NotEmpty()
            .WithMessage("Destination warehouse is required.");

        RuleFor(x => x.Request.Quantity)
            .GreaterThan(0)
            .WithMessage("Transfer quantity must be greater than zero.");

        RuleFor(x => x.Request)
            .Must(x => x.FromWarehouseId != x.ToWarehouseId)
            .WithMessage(
                "Source and destination warehouses must be different.");

        RuleFor(x => x.Request.Reference)
            .MaximumLength(200)
            .When(x => x.Request.Reference is not null);

        RuleFor(x => x.Request.Notes)
            .MaximumLength(500)
            .When(x => x.Request.Notes is not null);
    }
}