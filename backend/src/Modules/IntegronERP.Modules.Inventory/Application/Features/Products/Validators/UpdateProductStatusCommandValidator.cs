using FluentValidation;
using IntegronERP.Modules.Inventory.Application.Features.Products.Commands;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Validators;

public class UpdateProductStatusCommandValidator
    : AbstractValidator<UpdateProductStatusCommand>
{
    public UpdateProductStatusCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("Company information is required.");
    }
}