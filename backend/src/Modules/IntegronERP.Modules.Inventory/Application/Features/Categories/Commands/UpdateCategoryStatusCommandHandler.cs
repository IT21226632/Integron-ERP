using IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Categories.Commands;

public class UpdateCategoryStatusCommandHandler
    : IRequestHandler<
        UpdateCategoryStatusCommand,
        UpdateCategoryStatusResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryStatusCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateCategoryStatusResponse> Handle(
        UpdateCategoryStatusCommand command,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(
            command.Id,
            command.CompanyId,
            cancellationToken);

        if (category == null)
        {
            return new UpdateCategoryStatusResponse
            {
                Success = false,
                Message = "Category not found."
            };
        }

        category.IsActive = command.Request.IsActive;

        await _categoryRepository.UpdateAsync(
            category,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new UpdateCategoryStatusResponse
        {
            Success = true,
            Message = command.Request.IsActive
                ? "Category activated successfully."
                : "Category deactivated successfully."
        };
    }
}