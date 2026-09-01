using FluentValidation;
using IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Validators;

public class ReturnWarehouseStockCommandValidator
    : AbstractValidator<ReturnWarehouseStockCommand>
{
    public ReturnWarehouseStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("Company ID is required.");

        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("Warehouse ID is required.");

        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Request is required.");

        RuleFor(x => x.Request.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.Request.Reference)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Request.Reference))
            .WithMessage("Reference cannot exceed 100 characters.");

        RuleFor(x => x.Request.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Request.Notes))
            .WithMessage("Notes cannot exceed 500 characters.");
    }
}