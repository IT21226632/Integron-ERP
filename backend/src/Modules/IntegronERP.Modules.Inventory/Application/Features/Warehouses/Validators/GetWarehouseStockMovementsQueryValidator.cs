using FluentValidation;
using IntegronERP.Modules.Inventory.Application.Features.Warehouses.Queries;

namespace IntegronERP.Modules.Inventory.Application.Features.Warehouses.Validators;

public class GetWarehouseStockMovementsQueryValidator
    : AbstractValidator<GetWarehouseStockMovementsQuery>
{
    public GetWarehouseStockMovementsQueryValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("Warehouse is required.");

        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("Company information is required.");

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage(
                "Page must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(
                "Page size must be between 1 and 100.");

        RuleFor(x => x)
            .Must(x =>
                !x.FromDate.HasValue ||
                !x.ToDate.HasValue ||
                x.FromDate.Value <= x.ToDate.Value)
            .WithMessage(
                "From date cannot be later than to date.");
    }
}