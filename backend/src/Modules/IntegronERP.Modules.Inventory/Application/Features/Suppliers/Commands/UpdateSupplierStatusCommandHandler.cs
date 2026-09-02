using IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Suppliers.Commands;

public class UpdateSupplierStatusCommandHandler
    : IRequestHandler<
        UpdateSupplierStatusCommand,
        UpdateSupplierStatusResponse?>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSupplierStatusCommandHandler(
        ISupplierRepository supplierRepository,
        ICurrentUserService currentUser,
        IUnitOfWork unitOfWork)
    {
        _supplierRepository = supplierRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateSupplierStatusResponse?> Handle(
        UpdateSupplierStatusCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId;

        var supplier =
            await _supplierRepository.GetByIdAsync(
                request.Id,
                companyId,
                cancellationToken);

        if (supplier is null)
        {
            return null;
        }

        supplier.IsActive = request.IsActive;
        supplier.UpdatedAt = DateTime.UtcNow;

        await _supplierRepository.UpdateAsync(
            supplier,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new UpdateSupplierStatusResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            IsActive = supplier.IsActive,
            UpdatedAt = supplier.UpdatedAt
        };
    }
}