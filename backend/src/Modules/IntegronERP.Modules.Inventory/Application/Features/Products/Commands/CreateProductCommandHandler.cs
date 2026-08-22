using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;
using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Commands;

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateProductResponse> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // 1. Verify category exists, belongs to the company,
        //    and is currently active.
        var category =
            await _categoryRepository.GetActiveByIdAsync(
                request.CategoryId,
                command.CompanyId,
                cancellationToken);

        if (category is null)
        {
            return new CreateProductResponse
            {
                Success = false,
                Message = "Selected category does not exist or is inactive."
            };
        }

        // 2. Check SKU uniqueness within the company.
        var skuExists =
            await _productRepository.ExistsBySkuAsync(
                command.CompanyId,
                request.SKU.Trim(),
                cancellationToken);

        if (skuExists)
        {
            return new CreateProductResponse
            {
                Success = false,
                Message = "A product with this SKU already exists."
            };
        }

        // 3. Create product.
        var product = new Product
        {
            Id = Guid.NewGuid(),
            CompanyId = command.CompanyId,
            CategoryId = category.Id,
            Name = request.Name.Trim(),
            SKU = request.SKU.Trim(),
            Description = request.Description?.Trim(),
            UnitPrice = request.UnitPrice,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // 4. Persist product.
        await _productRepository.AddAsync(
            product,
            cancellationToken);

        // 5. Commit transaction.
        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new CreateProductResponse
        {
            Success = true,
            Message = "Product created successfully.",
            ProductId = product.Id
        };
    }
}