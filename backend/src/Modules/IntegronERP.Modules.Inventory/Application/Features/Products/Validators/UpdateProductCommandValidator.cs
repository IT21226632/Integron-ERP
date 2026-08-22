using FluentValidation;
using IntegronERP.Modules.Inventory.Application.Features.Products.Commands;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Validators;

public class UpdateProductCommandValidator
    : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("Company ID is required.");

        RuleFor(x => x.Request.CategoryId)
            .NotEmpty()
            .WithMessage("Category is required.");

        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(200)
            .WithMessage("Product name cannot exceed 200 characters.");

        RuleFor(x => x.Request.SKU)
            .NotEmpty()
            .WithMessage("SKU is required.")
            .MaximumLength(100)
            .WithMessage("SKU cannot exceed 100 characters.");

        RuleFor(x => x.Request.UnitPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Unit price cannot be negative.");
    }
}