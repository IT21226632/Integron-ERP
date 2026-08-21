using IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Categories.Commands;

public class UpdateCategoryCommandHandler
    : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCategoryResponse> Handle(
        UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(
            command.Id,
            command.CompanyId,
            cancellationToken);

        if (category == null)
        {
            return new UpdateCategoryResponse
            {
                Success = false,
                Message = "Category not found."
            };
        }

        var duplicateExists =
            await _categoryRepository.ExistsByNameAsync(
                command.CompanyId,
                command.Request.Name,
                cancellationToken);

        if (duplicateExists &&
            !string.Equals(
                category.Name,
                command.Request.Name,
                StringComparison.OrdinalIgnoreCase))
        {
            return new UpdateCategoryResponse
            {
                Success = false,
                Message = "A category with this name already exists."
            };
        }

        category.Name = command.Request.Name.Trim();
        category.Description = command.Request.Description?.Trim();

        await _categoryRepository.UpdateAsync(
            category,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new UpdateCategoryResponse
        {
            Success = true,
            Message = "Category updated successfully.",
            Category = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt
            }
        };
    }
}