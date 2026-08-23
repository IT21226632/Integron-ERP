using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Products.Commands;

public class UpdateProductStatusCommandHandler
    : IRequestHandler<
        UpdateProductStatusCommand,
        UpdateProductStatusResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductStatusCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateProductStatusResponse> Handle(
        UpdateProductStatusCommand command,
        CancellationToken cancellationToken)
    {
        var product =
            await _productRepository.GetByIdAsync(
                command.ProductId,
                command.CompanyId,
                cancellationToken);

        if (product is null)
        {
            return new UpdateProductStatusResponse
            {
                Success = false,
                Message = "Product not found."
            };
        }

        product.IsActive = command.Request.IsActive;

        await _productRepository.UpdateAsync(
            product,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new UpdateProductStatusResponse
        {
            Success = true,
            Message = command.Request.IsActive
                ? "Product activated successfully."
                : "Product deactivated successfully.",
            ProductId = product.Id,
            IsActive = product.IsActive
        };
    }
}