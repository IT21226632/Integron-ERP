using FluentValidation;
using IntegronERP.Modules.Inventory.Application.Features.Categories.Commands;

namespace IntegronERP.Modules.Inventory.Application.Features.Categories.Validators;

public class UpdateCategoryCommandValidator
    : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .WithMessage("Category name is required.")
            .MaximumLength(100)
            .WithMessage("Category name cannot exceed 100 characters.");

        RuleFor(x => x.Request.Description)
            .MaximumLength(500)
            .WithMessage("Category description cannot exceed 500 characters.");
    }
}