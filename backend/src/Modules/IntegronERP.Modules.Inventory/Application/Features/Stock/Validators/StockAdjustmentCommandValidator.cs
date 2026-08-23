using FluentValidation;
using IntegronERP.Modules.Inventory.Application.Features.Stock.Commands;

namespace IntegronERP.Modules.Inventory.Application.Features.Stock.Validators;

public class StockAdjustmentCommandValidator
    : AbstractValidator<StockAdjustmentCommand>
{
    public StockAdjustmentCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("Company information is required.");

        RuleFor(x => x.Request.Quantity)
            .NotEqual(0)
            .WithMessage("Adjustment quantity cannot be zero.");

        RuleFor(x => x.Request.Reference)
            .MaximumLength(200)
            .When(x => x.Request.Reference != null)
            .WithMessage("Reference cannot exceed 200 characters.");

        RuleFor(x => x.Request.Notes)
            .MaximumLength(1000)
            .When(x => x.Request.Notes != null)
            .WithMessage("Notes cannot exceed 1000 characters.");
    }
}