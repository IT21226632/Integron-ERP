using IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Suppliers.Commands;

public class UpdateSupplierCommandHandler
    : IRequestHandler<
        UpdateSupplierCommand,
        UpdateSupplierResponse?>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSupplierCommandHandler(
        ISupplierRepository supplierRepository,
        ICurrentUserService currentUser,
        IUnitOfWork unitOfWork)
    {
        _supplierRepository = supplierRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateSupplierResponse?> Handle(
        UpdateSupplierCommand request,
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

        var nameExists =
            await _supplierRepository.ExistsByNameAsync(
                companyId,
                request.Name.Trim(),
                request.Id,
                cancellationToken);

        if (nameExists)
        {
            throw new InvalidOperationException(
                "A supplier with this name already exists.");
        }

        supplier.Name = request.Name.Trim();
        supplier.Email = request.Email?.Trim();
        supplier.PhoneNumber = request.PhoneNumber?.Trim();
        supplier.ContactPerson = request.ContactPerson?.Trim();
        supplier.Address = request.Address?.Trim();
        supplier.UpdatedAt = DateTime.UtcNow;

        await _supplierRepository.UpdateAsync(
            supplier,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new UpdateSupplierResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Email = supplier.Email,
            PhoneNumber = supplier.PhoneNumber,
            ContactPerson = supplier.ContactPerson,
            Address = supplier.Address,
            IsActive = supplier.IsActive,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt
        };
    }
}