using FluentValidation;
using IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Validators;

public class CreateWarehouseCommandValidator
    : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator()
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Request.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches("^[A-Za-z0-9_-]+$")
            .WithMessage(
                "Warehouse code can only contain letters, numbers, underscores, and hyphens.");

        RuleFor(x => x.Request.Address)
            .MaximumLength(500);
    }
}