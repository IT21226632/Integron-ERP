using IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;
using IntegronERP.Modules.Inventory.Domain.Entities;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Suppliers.Commands;

public class CreateSupplierCommandHandler
    : IRequestHandler<CreateSupplierCommand, CreateSupplierResponse>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSupplierCommandHandler(
        ISupplierRepository supplierRepository,
        ICurrentUserService currentUser,
        IUnitOfWork unitOfWork)
    {
        _supplierRepository = supplierRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateSupplierResponse> Handle(
        CreateSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId;

        var nameExists =
            await _supplierRepository.ExistsByNameAsync(
                companyId,
                request.Name,
                cancellationToken);

        if (nameExists)
        {
            throw new InvalidOperationException(
                "A supplier with this name already exists.");
        }

        var supplier = new Supplier
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            Name = request.Name.Trim(),
            Email = request.Email?.Trim(),
            PhoneNumber = request.PhoneNumber?.Trim(),
            ContactPerson = request.ContactPerson?.Trim(),
            Address = request.Address?.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _supplierRepository.AddAsync(
            supplier,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new CreateSupplierResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Email = supplier.Email,
            PhoneNumber = supplier.PhoneNumber,
            ContactPerson = supplier.ContactPerson,
            Address = supplier.Address,
            IsActive = supplier.IsActive,
            CreatedAt = supplier.CreatedAt
        };
    }
}