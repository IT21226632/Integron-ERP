using IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;
using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Categories.Commands;

public class CreateCategoryCommandHandler
    : IRequestHandler<
        CreateCategoryCommand,
        CreateCategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCategoryResponse> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        var exists = await _categoryRepository.ExistsByNameAsync(
            command.CompanyId,
            request.Name,
            cancellationToken);

        if (exists)
        {
            return new CreateCategoryResponse
            {
                Success = false,
                Message = "A category with this name already exists."
            };
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            CompanyId = command.CompanyId,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _categoryRepository.AddAsync(
            category,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new CreateCategoryResponse
        {
            Success = true,
            Message = "Category created successfully.",
            CategoryId = category.Id
        };
    }
}