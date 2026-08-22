using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Commands;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, UpdateProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateProductResponse> Handle(
        UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // 1. Find product within the authenticated company.
        var product =
            await _productRepository.GetByIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        if (product is null)
        {
            return new UpdateProductResponse
            {
                Success = false,
                Message = "Product not found."
            };
        }

        // 2. Verify category belongs to the company and is active.
        var category =
            await _categoryRepository.GetActiveByIdAsync(
                request.CategoryId,
                command.CompanyId,
                cancellationToken);

        if (category is null)
        {
            return new UpdateProductResponse
            {
                Success = false,
                Message = "Selected category does not exist or is inactive."
            };
        }

        // 3. Check SKU uniqueness excluding current product.
        var skuExists =
            await _productRepository.ExistsBySkuAsync(
                command.CompanyId,
                request.SKU.Trim(),
                command.ProductId,
                cancellationToken);

        if (skuExists)
        {
            return new UpdateProductResponse
            {
                Success = false,
                Message = "A product with this SKU already exists."
            };
        }

        // 4. Update product fields.
        product.CategoryId = category.Id;
        product.Name = request.Name.Trim();
        product.SKU = request.SKU.Trim();
        product.Description = request.Description?.Trim();
        product.UnitPrice = request.UnitPrice;

        // 5. Persist changes.
        await _productRepository.UpdateAsync(
            product,
            cancellationToken);

        // 6. Commit.
        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new UpdateProductResponse
        {
            Success = true,
            Message = "Product updated successfully.",
            ProductId = product.Id
        };
    }
}