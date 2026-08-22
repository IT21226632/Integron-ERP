using FluentValidation;
using IntegronERP.Modules.Inventory.Application.Features.Products.Commands;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Validators;

public class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
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

        RuleFor(x => x.Request.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.")
            .When(x => x.Request.Description != null);

        RuleFor(x => x.Request.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero.");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("Company information is required.");
    }
}